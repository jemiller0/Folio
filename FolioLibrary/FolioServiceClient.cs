using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Web;

namespace FolioLibrary
{
    public class FolioServiceClient : IDisposable
    {
        public string AccessToken { get; set; }
        private Formatting formatting = traceSource.Switch.Level == SourceLevels.Verbose ? Formatting.Indented : Formatting.None;
        private HttpClient httpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }) { Timeout = Timeout.InfiniteTimeSpan };
        private readonly static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented };
        public string Password { get; set; }
        public string Tenant { get; set; }
        public readonly static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.Information);
        public string Username { get; set; }
        public string Url { get; set; }

        public FolioServiceClient(string nameOrConnectionString = "FolioServiceClient")
        {
            var connectionString = ConfigurationManager.ConnectionStrings[nameOrConnectionString ?? "FolioServiceClient"]?.ConnectionString ?? nameOrConnectionString;
            if (connectionString != null && connectionString.StartsWith("http"))
            {
                var u = new Uri(connectionString);
                Url = $"{u.Scheme}://{u.Authority}";
                Tenant = u.LocalPath.Substring(1);
                var nvc = HttpUtility.ParseQueryString(u.Query);
                Username = nvc["Username"];
                Password = nvc["Password"];
                AccessToken = nvc["AccessToken"];
            }
            lock (this)
            {
                httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                httpClient.DefaultRequestHeaders.Accept.ParseAdd("text/plain");
                httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-tenant", Tenant);
                if (AccessToken != null) httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-token", AccessToken);
            }
        }

        private void AuthenticateIfNecessary()
        {
            lock (this)
            {
                if (AccessToken == null) Authenticate();
            }
        }

        public string Authenticate(string username = null, string password = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Authenticating");
            if (username != null) Username = username;
            if (Username == null) throw new ArgumentNullException(nameof(Username));
            if (password != null) Password = password;
            if (Password == null) throw new ArgumentNullException(nameof(Password));
            var url = $"{Url}/authn/login";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = JsonConvert.SerializeObject(new { Username, Password }, jsonSerializerSettings);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, new StringContent(s2, Encoding.UTF8, "application/json")).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.Created)
                if (s2 == "Bad credentials")
                    throw new InvalidCredentialException(s2);
                else
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            AccessToken = hrm.Headers.GetValues("x-okapi-token").Single();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-token", AccessToken);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return AccessToken;
        }

        public int CountAcquisitionsUnits(string where = null)
        {
            AcquisitionsUnits(out var i, take: 0);
            return i;
        }

        public JObject[] AcquisitionsUnits(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying acquisitions units");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/acquisitions-units-storage/units{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> AcquisitionsUnits(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying acquisitions units");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/units{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetAcquisitionsUnit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting acquisitions unit {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/units/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertAcquisitionsUnit(JObject acquisitionsUnit)
        {
            var s = Stopwatch.StartNew();
            if (acquisitionsUnit["id"] == null) acquisitionsUnit["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting acquisitions unit {0}", acquisitionsUnit["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/units";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = acquisitionsUnit.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateAcquisitionsUnit(JObject acquisitionsUnit)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating acquisitions unit {acquisitionsUnit["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/units/{acquisitionsUnit["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = acquisitionsUnit.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteAcquisitionsUnit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting acquisitions unit {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/units/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountAddressTypes(string where = null)
        {
            AddressTypes(out var i, take: 0);
            return i;
        }

        public JObject[] AddressTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying address types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/addresstypes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> AddressTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying address types");
            AuthenticateIfNecessary();
            var url = $"{Url}/addresstypes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetAddressType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting address type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/addresstypes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertAddressType(JObject addressType)
        {
            var s = Stopwatch.StartNew();
            if (addressType["id"] == null) addressType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting address type {0}", addressType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/addresstypes";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = addressType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateAddressType(JObject addressType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating address type {addressType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/addresstypes/{addressType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = addressType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteAddressType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting address type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/addresstypes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountAlerts(string where = null)
        {
            Alerts(out var i, take: 0);
            return i;
        }

        public JObject[] Alerts(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying alerts");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/orders-storage/alerts{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Alerts(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying alerts");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/alerts{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetAlert(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting alert {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/alerts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertAlert(JObject alert)
        {
            var s = Stopwatch.StartNew();
            if (alert["id"] == null) alert["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting alert {0}", alert["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/alerts";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = alert.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateAlert(JObject alert)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating alert {alert["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/alerts/{alert["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = alert.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteAlert(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting alert {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/alerts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountAlternativeTitleTypes(string where = null)
        {
            AlternativeTitleTypes(out var i, take: 0);
            return i;
        }

        public JObject[] AlternativeTitleTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying alternative title types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/alternative-title-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> AlternativeTitleTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying alternative title types");
            AuthenticateIfNecessary();
            var url = $"{Url}/alternative-title-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetAlternativeTitleType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting alternative title type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/alternative-title-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertAlternativeTitleType(JObject alternativeTitleType)
        {
            var s = Stopwatch.StartNew();
            if (alternativeTitleType["id"] == null) alternativeTitleType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting alternative title type {0}", alternativeTitleType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/alternative-title-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = alternativeTitleType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateAlternativeTitleType(JObject alternativeTitleType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating alternative title type {alternativeTitleType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/alternative-title-types/{alternativeTitleType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = alternativeTitleType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteAlternativeTitleType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting alternative title type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/alternative-title-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountBatchGroups(string where = null)
        {
            BatchGroups(out var i, take: 0);
            return i;
        }

        public JObject[] BatchGroups(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying batch groups");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/batch-group-storage/batch-groups{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> BatchGroups(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying batch groups");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-group-storage/batch-groups{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetBatchGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting batch group {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-group-storage/batch-groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertBatchGroup(JObject batchGroup)
        {
            var s = Stopwatch.StartNew();
            if (batchGroup["id"] == null) batchGroup["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting batch group {0}", batchGroup["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-group-storage/batch-groups";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = batchGroup.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateBatchGroup(JObject batchGroup)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating batch group {batchGroup["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-group-storage/batch-groups/{batchGroup["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = batchGroup.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteBatchGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting batch group {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-group-storage/batch-groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountBatchVoucherExports(string where = null)
        {
            BatchVoucherExports(out var i, take: 0);
            return i;
        }

        public JObject[] BatchVoucherExports(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying batch voucher exports");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/batch-voucher-storage/batch-voucher-exports{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> BatchVoucherExports(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying batch voucher exports");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/batch-voucher-exports{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetBatchVoucherExport(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting batch voucher export {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/batch-voucher-exports/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertBatchVoucherExport(JObject batchVoucherExport)
        {
            var s = Stopwatch.StartNew();
            if (batchVoucherExport["id"] == null) batchVoucherExport["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting batch voucher export {0}", batchVoucherExport["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/batch-voucher-exports";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = batchVoucherExport.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateBatchVoucherExport(JObject batchVoucherExport)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating batch voucher export {batchVoucherExport["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/batch-voucher-exports/{batchVoucherExport["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = batchVoucherExport.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteBatchVoucherExport(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting batch voucher export {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/batch-voucher-exports/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountBatchVoucherExportConfigs(string where = null)
        {
            BatchVoucherExportConfigs(out var i, take: 0);
            return i;
        }

        public JObject[] BatchVoucherExportConfigs(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying batch voucher export configs");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/batch-voucher-storage/export-configurations{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> BatchVoucherExportConfigs(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying batch voucher export configs");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/export-configurations{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetBatchVoucherExportConfig(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting batch voucher export config {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/export-configurations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertBatchVoucherExportConfig(JObject batchVoucherExportConfig)
        {
            var s = Stopwatch.StartNew();
            if (batchVoucherExportConfig["id"] == null) batchVoucherExportConfig["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting batch voucher export config {0}", batchVoucherExportConfig["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/export-configurations";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = batchVoucherExportConfig.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateBatchVoucherExportConfig(JObject batchVoucherExportConfig)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating batch voucher export config {batchVoucherExportConfig["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/export-configurations/{batchVoucherExportConfig["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = batchVoucherExportConfig.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteBatchVoucherExportConfig(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting batch voucher export config {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/export-configurations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountBlocks(string where = null)
        {
            Blocks(out var i, take: 0);
            return i;
        }

        public JObject[] Blocks(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying blocks");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/manualblocks{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Blocks(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying blocks");
            AuthenticateIfNecessary();
            var url = $"{Url}/manualblocks{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetBlock(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting block {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/manualblocks/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertBlock(JObject block)
        {
            var s = Stopwatch.StartNew();
            if (block["id"] == null) block["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting block {0}", block["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/manualblocks";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = block.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateBlock(JObject block)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating block {block["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/manualblocks/{block["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = block.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteBlock(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting block {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/manualblocks/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountBlockConditions(string where = null)
        {
            BlockConditions(out var i, take: 0);
            return i;
        }

        public JObject[] BlockConditions(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying block conditions");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/patron-block-conditions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> BlockConditions(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying block conditions");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-conditions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetBlockCondition(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting block condition {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-conditions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertBlockCondition(JObject blockCondition)
        {
            var s = Stopwatch.StartNew();
            if (blockCondition["id"] == null) blockCondition["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting block condition {0}", blockCondition["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-conditions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = blockCondition.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateBlockCondition(JObject blockCondition)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating block condition {blockCondition["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-conditions/{blockCondition["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = blockCondition.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteBlockCondition(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting block condition {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-conditions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountBlockLimits(string where = null)
        {
            BlockLimits(out var i, take: 0);
            return i;
        }

        public JObject[] BlockLimits(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying block limits");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/patron-block-limits{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> BlockLimits(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying block limits");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-limits{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetBlockLimit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting block limit {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-limits/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertBlockLimit(JObject blockLimit)
        {
            var s = Stopwatch.StartNew();
            if (blockLimit["id"] == null) blockLimit["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting block limit {0}", blockLimit["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-limits";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = blockLimit.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateBlockLimit(JObject blockLimit)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating block limit {blockLimit["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-limits/{blockLimit["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = blockLimit.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteBlockLimit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting block limit {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-limits/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountBudgets(string where = null)
        {
            Budgets(out var i, take: 0);
            return i;
        }

        public JObject[] Budgets(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying budgets");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/finance-storage/budgets{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Budgets(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying budgets");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/budgets{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetBudget(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting budget {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/budgets/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertBudget(JObject budget)
        {
            var s = Stopwatch.StartNew();
            if (budget["id"] == null) budget["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting budget {0}", budget["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/budgets";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = budget.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateBudget(JObject budget)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating budget {budget["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/budgets/{budget["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = budget.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteBudget(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting budget {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/budgets/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountCallNumberTypes(string where = null)
        {
            CallNumberTypes(out var i, take: 0);
            return i;
        }

        public JObject[] CallNumberTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying call number types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/call-number-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> CallNumberTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying call number types");
            AuthenticateIfNecessary();
            var url = $"{Url}/call-number-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetCallNumberType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting call number type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/call-number-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertCallNumberType(JObject callNumberType)
        {
            var s = Stopwatch.StartNew();
            if (callNumberType["id"] == null) callNumberType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting call number type {0}", callNumberType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/call-number-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = callNumberType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateCallNumberType(JObject callNumberType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating call number type {callNumberType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/call-number-types/{callNumberType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = callNumberType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteCallNumberType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting call number type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/call-number-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountCampuses(string where = null)
        {
            Campuses(out var i, take: 0);
            return i;
        }

        public JObject[] Campuses(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying campuses");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/location-units/campuses{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Campuses(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying campuses");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/campuses{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetCampus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting campus {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/campuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertCampus(JObject campus)
        {
            var s = Stopwatch.StartNew();
            if (campus["id"] == null) campus["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting campus {0}", campus["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/campuses";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = campus.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateCampus(JObject campus)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating campus {campus["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/campuses/{campus["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = campus.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteCampus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting campus {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/campuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountCancellationReasons(string where = null)
        {
            CancellationReasons(out var i, take: 0);
            return i;
        }

        public JObject[] CancellationReasons(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying cancellation reasons");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/cancellation-reason-storage/cancellation-reasons{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> CancellationReasons(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying cancellation reasons");
            AuthenticateIfNecessary();
            var url = $"{Url}/cancellation-reason-storage/cancellation-reasons{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetCancellationReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting cancellation reason {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/cancellation-reason-storage/cancellation-reasons/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertCancellationReason(JObject cancellationReason)
        {
            var s = Stopwatch.StartNew();
            if (cancellationReason["id"] == null) cancellationReason["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting cancellation reason {0}", cancellationReason["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/cancellation-reason-storage/cancellation-reasons";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = cancellationReason.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateCancellationReason(JObject cancellationReason)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating cancellation reason {cancellationReason["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/cancellation-reason-storage/cancellation-reasons/{cancellationReason["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = cancellationReason.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteCancellationReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting cancellation reason {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/cancellation-reason-storage/cancellation-reasons/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountCategories(string where = null)
        {
            Categories(out var i, take: 0);
            return i;
        }

        public JObject[] Categories(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying categories");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/organizations-storage/categories{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Categories(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying categories");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/categories{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetCategory(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting category {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/categories/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertCategory(JObject category)
        {
            var s = Stopwatch.StartNew();
            if (category["id"] == null) category["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting category {0}", category["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/categories";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = category.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateCategory(JObject category)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating category {category["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/categories/{category["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = category.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteCategory(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting category {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/categories/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountCheckIns(string where = null)
        {
            CheckIns(out var i, take: 0);
            return i;
        }

        public JObject[] CheckIns(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying check ins");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/check-in-storage/check-ins{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> CheckIns(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying check ins");
            AuthenticateIfNecessary();
            var url = $"{Url}/check-in-storage/check-ins{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetCheckIn(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting check in {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/check-in-storage/check-ins/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertCheckIn(JObject checkIn)
        {
            var s = Stopwatch.StartNew();
            if (checkIn["id"] == null) checkIn["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting check in {0}", checkIn["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/check-in-storage/check-ins";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = checkIn.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateCheckIn(JObject checkIn)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating check in {checkIn["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/check-in-storage/check-ins/{checkIn["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = checkIn.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteCheckIn(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting check in {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/check-in-storage/check-ins/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetCirculationRule()
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting circulation rule");
            AuthenticateIfNecessary();
            var url = $"{Url}/circulation-rules-storage";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateCirculationRule(JObject circulationRule)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating circulation rule {circulationRule["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/circulation-rules-storage";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = circulationRule.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountClassificationTypes(string where = null)
        {
            ClassificationTypes(out var i, take: 0);
            return i;
        }

        public JObject[] ClassificationTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying classification types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/classification-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> ClassificationTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying classification types");
            AuthenticateIfNecessary();
            var url = $"{Url}/classification-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetClassificationType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting classification type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/classification-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertClassificationType(JObject classificationType)
        {
            var s = Stopwatch.StartNew();
            if (classificationType["id"] == null) classificationType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting classification type {0}", classificationType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/classification-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = classificationType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateClassificationType(JObject classificationType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating classification type {classificationType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/classification-types/{classificationType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = classificationType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteClassificationType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting classification type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/classification-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountCloseReasons(string where = null)
        {
            CloseReasons(out var i, take: 0);
            return i;
        }

        public JObject[] CloseReasons(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying close reasons");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/orders-storage/configuration/reasons-for-closure{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> CloseReasons(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying close reasons");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/reasons-for-closure{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetCloseReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting close reason {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/reasons-for-closure/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertCloseReason(JObject closeReason)
        {
            var s = Stopwatch.StartNew();
            if (closeReason["id"] == null) closeReason["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting close reason {0}", closeReason["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/reasons-for-closure";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = closeReason.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateCloseReason(JObject closeReason)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating close reason {closeReason["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/reasons-for-closure/{closeReason["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = closeReason.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteCloseReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting close reason {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/reasons-for-closure/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountComments(string where = null)
        {
            Comments(out var i, take: 0);
            return i;
        }

        public JObject[] Comments(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying comments");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/comments{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Comments(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying comments");
            AuthenticateIfNecessary();
            var url = $"{Url}/comments{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetComment(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting comment {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/comments/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertComment(JObject comment)
        {
            var s = Stopwatch.StartNew();
            if (comment["id"] == null) comment["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting comment {0}", comment["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/comments";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = comment.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateComment(JObject comment)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating comment {comment["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/comments/{comment["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = comment.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteComment(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting comment {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/comments/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountConfigurations(string where = null)
        {
            Configurations(out var i, take: 0);
            return i;
        }

        public JObject[] Configurations(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying configurations");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/configurations/entries{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Configurations(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying configurations");
            AuthenticateIfNecessary();
            var url = $"{Url}/configurations/entries{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetConfiguration(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting configuration {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/configurations/entries/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertConfiguration(JObject configuration)
        {
            var s = Stopwatch.StartNew();
            if (configuration["id"] == null) configuration["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting configuration {0}", configuration["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/configurations/entries";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = configuration.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateConfiguration(JObject configuration)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating configuration {configuration["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/configurations/entries/{configuration["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = configuration.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteConfiguration(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting configuration {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/configurations/entries/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountContacts(string where = null)
        {
            Contacts(out var i, take: 0);
            return i;
        }

        public JObject[] Contacts(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying contacts");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/organizations-storage/contacts{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Contacts(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying contacts");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/contacts{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetContact(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting contact {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/contacts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertContact(JObject contact)
        {
            var s = Stopwatch.StartNew();
            if (contact["id"] == null) contact["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting contact {0}", contact["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/contacts";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = contact.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateContact(JObject contact)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating contact {contact["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/contacts/{contact["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = contact.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteContact(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting contact {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/contacts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountContributorNameTypes(string where = null)
        {
            ContributorNameTypes(out var i, take: 0);
            return i;
        }

        public JObject[] ContributorNameTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying contributor name types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/contributor-name-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> ContributorNameTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying contributor name types");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-name-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetContributorNameType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting contributor name type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-name-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertContributorNameType(JObject contributorNameType)
        {
            var s = Stopwatch.StartNew();
            if (contributorNameType["id"] == null) contributorNameType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting contributor name type {0}", contributorNameType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-name-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = contributorNameType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateContributorNameType(JObject contributorNameType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating contributor name type {contributorNameType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-name-types/{contributorNameType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = contributorNameType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteContributorNameType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting contributor name type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-name-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountContributorTypes(string where = null)
        {
            ContributorTypes(out var i, take: 0);
            return i;
        }

        public JObject[] ContributorTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying contributor types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/contributor-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> ContributorTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying contributor types");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetContributorType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting contributor type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertContributorType(JObject contributorType)
        {
            var s = Stopwatch.StartNew();
            if (contributorType["id"] == null) contributorType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting contributor type {0}", contributorType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = contributorType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateContributorType(JObject contributorType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating contributor type {contributorType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-types/{contributorType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = contributorType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteContributorType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting contributor type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountCustomFields(string where = null)
        {
            CustomFields(out var i, take: 0);
            return i;
        }

        public JObject[] CustomFields(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying custom fields");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/custom-fields{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-module-id", "mod-users-17.1.0");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            httpClient.DefaultRequestHeaders.Remove("x-okapi-module-id");
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> CustomFields(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying custom fields");
            AuthenticateIfNecessary();
            var url = $"{Url}/custom-fields{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-module-id", "mod-users-17.1.0");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            httpClient.DefaultRequestHeaders.Remove("x-okapi-module-id");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetCustomField(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting custom field {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/custom-fields/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-module-id", "mod-users-17.1.0");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            httpClient.DefaultRequestHeaders.Remove("x-okapi-module-id");
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertCustomField(JObject customField)
        {
            var s = Stopwatch.StartNew();
            if (customField["id"] == null) customField["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting custom field {0}", customField["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/custom-fields";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = customField.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-module-id", "mod-users-17.1.0");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            httpClient.DefaultRequestHeaders.Remove("x-okapi-module-id");
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateCustomField(JObject customField)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating custom field {customField["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/custom-fields/{customField["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = customField.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-module-id", "mod-users-17.1.0");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            httpClient.DefaultRequestHeaders.Remove("x-okapi-module-id");
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteCustomField(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting custom field {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/custom-fields/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-module-id", "mod-users-17.1.0");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            httpClient.DefaultRequestHeaders.Remove("x-okapi-module-id");
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountElectronicAccessRelationships(string where = null)
        {
            ElectronicAccessRelationships(out var i, take: 0);
            return i;
        }

        public JObject[] ElectronicAccessRelationships(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying electronic access relationships");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/electronic-access-relationships{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> ElectronicAccessRelationships(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying electronic access relationships");
            AuthenticateIfNecessary();
            var url = $"{Url}/electronic-access-relationships{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetElectronicAccessRelationship(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting electronic access relationship {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/electronic-access-relationships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertElectronicAccessRelationship(JObject electronicAccessRelationship)
        {
            var s = Stopwatch.StartNew();
            if (electronicAccessRelationship["id"] == null) electronicAccessRelationship["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting electronic access relationship {0}", electronicAccessRelationship["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/electronic-access-relationships";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = electronicAccessRelationship.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateElectronicAccessRelationship(JObject electronicAccessRelationship)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating electronic access relationship {electronicAccessRelationship["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/electronic-access-relationships/{electronicAccessRelationship["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = electronicAccessRelationship.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteElectronicAccessRelationship(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting electronic access relationship {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/electronic-access-relationships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountFees(string where = null)
        {
            Fees(out var i, take: 0);
            return i;
        }

        public JObject[] Fees(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fees");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/accounts{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Fees(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fees");
            AuthenticateIfNecessary();
            skip = skip ?? 0;
            take = take ?? int.MaxValue;
            orderBy = orderBy ?? "id";
            while (take > 0)
            {
                var url = $"{Url}/accounts{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={Math.Min(take.Value, 10000)}";
                traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
                var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
                if (hrm.StatusCode != HttpStatusCode.OK)
                {
                    var s2 = hrm.Content.ReadAsStringAsync().Result;
                    if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
                }
                using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
                using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
                {
                    if (!jtr.Read()) throw new InvalidDataException();
                    while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                    var js = new JsonSerializer();
                    var i = 0;
                    while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                    {
                        var jo = (JObject)js.Deserialize(jtr);
                        traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                        ++i;
                        yield return jo;
                    }
                    if (i < Math.Min(take.Value, 10000)) break;
                    skip += i;
                    take -= i;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetFee(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting fee {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/accounts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertFee(JObject fee)
        {
            var s = Stopwatch.StartNew();
            if (fee["id"] == null) fee["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting fee {0}", fee["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/accounts";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fee.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateFee(JObject fee)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating fee {fee["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/accounts/{fee["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fee.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteFee(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fee {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/accounts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountFeeTypes(string where = null)
        {
            FeeTypes(out var i, take: 0);
            return i;
        }

        public JObject[] FeeTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fee types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/feefines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> FeeTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fee types");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetFeeType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting fee type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/feefines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertFeeType(JObject feeType)
        {
            var s = Stopwatch.StartNew();
            if (feeType["id"] == null) feeType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting fee type {0}", feeType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/feefines";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = feeType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateFeeType(JObject feeType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating fee type {feeType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefines/{feeType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = feeType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteFeeType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fee type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountFinanceGroups(string where = null)
        {
            FinanceGroups(out var i, take: 0);
            return i;
        }

        public JObject[] FinanceGroups(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying finance groups");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/finance-storage/groups{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> FinanceGroups(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying finance groups");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/groups{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetFinanceGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting finance group {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertFinanceGroup(JObject financeGroup)
        {
            var s = Stopwatch.StartNew();
            if (financeGroup["id"] == null) financeGroup["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting finance group {0}", financeGroup["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/groups";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = financeGroup.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateFinanceGroup(JObject financeGroup)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating finance group {financeGroup["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/groups/{financeGroup["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = financeGroup.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteFinanceGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting finance group {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountFiscalYears(string where = null)
        {
            FiscalYears(out var i, take: 0);
            return i;
        }

        public JObject[] FiscalYears(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fiscal years");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/finance-storage/fiscal-years{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> FiscalYears(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fiscal years");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fiscal-years{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetFiscalYear(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting fiscal year {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fiscal-years/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertFiscalYear(JObject fiscalYear)
        {
            var s = Stopwatch.StartNew();
            if (fiscalYear["id"] == null) fiscalYear["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting fiscal year {0}", fiscalYear["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fiscal-years";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fiscalYear.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateFiscalYear(JObject fiscalYear)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating fiscal year {fiscalYear["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fiscal-years/{fiscalYear["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fiscalYear.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteFiscalYear(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fiscal year {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fiscal-years/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountFixedDueDateSchedules(string where = null)
        {
            FixedDueDateSchedules(out var i, take: 0);
            return i;
        }

        public JObject[] FixedDueDateSchedules(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fixed due date schedules");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/fixed-due-date-schedule-storage/fixed-due-date-schedules{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> FixedDueDateSchedules(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fixed due date schedules");
            AuthenticateIfNecessary();
            var url = $"{Url}/fixed-due-date-schedule-storage/fixed-due-date-schedules{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetFixedDueDateSchedule(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting fixed due date schedule {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/fixed-due-date-schedule-storage/fixed-due-date-schedules/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertFixedDueDateSchedule(JObject fixedDueDateSchedule)
        {
            var s = Stopwatch.StartNew();
            if (fixedDueDateSchedule["id"] == null) fixedDueDateSchedule["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting fixed due date schedule {0}", fixedDueDateSchedule["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/fixed-due-date-schedule-storage/fixed-due-date-schedules";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fixedDueDateSchedule.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateFixedDueDateSchedule(JObject fixedDueDateSchedule)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating fixed due date schedule {fixedDueDateSchedule["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/fixed-due-date-schedule-storage/fixed-due-date-schedules/{fixedDueDateSchedule["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fixedDueDateSchedule.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteFixedDueDateSchedule(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fixed due date schedule {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/fixed-due-date-schedule-storage/fixed-due-date-schedules/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountFunds(string where = null)
        {
            Funds(out var i, take: 0);
            return i;
        }

        public JObject[] Funds(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying funds");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/finance-storage/funds{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Funds(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying funds");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/funds{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetFund(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting fund {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/funds/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertFund(JObject fund)
        {
            var s = Stopwatch.StartNew();
            if (fund["id"] == null) fund["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting fund {0}", fund["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/funds";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fund.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateFund(JObject fund)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating fund {fund["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/funds/{fund["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fund.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteFund(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fund {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/funds/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountFundTypes(string where = null)
        {
            FundTypes(out var i, take: 0);
            return i;
        }

        public JObject[] FundTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fund types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/finance-storage/fund-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> FundTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fund types");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fund-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetFundType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting fund type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fund-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertFundType(JObject fundType)
        {
            var s = Stopwatch.StartNew();
            if (fundType["id"] == null) fundType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting fund type {0}", fundType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fund-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fundType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateFundType(JObject fundType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating fund type {fundType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fund-types/{fundType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fundType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteFundType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fund type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fund-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountGroups(string where = null)
        {
            Groups(out var i, take: 0);
            return i;
        }

        public JObject[] Groups(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying groups");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/groups{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Groups(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying groups");
            AuthenticateIfNecessary();
            var url = $"{Url}/groups{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting group {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertGroup(JObject group)
        {
            var s = Stopwatch.StartNew();
            if (group["id"] == null) group["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting group {0}", group["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/groups";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = group.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateGroup(JObject group)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating group {group["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/groups/{group["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = group.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting group {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountGroupFundFiscalYears(string where = null)
        {
            GroupFundFiscalYears(out var i, take: 0);
            return i;
        }

        public JObject[] GroupFundFiscalYears(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying group fund fiscal years");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/finance-storage/group-fund-fiscal-years{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> GroupFundFiscalYears(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying group fund fiscal years");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/group-fund-fiscal-years{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetGroupFundFiscalYear(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting group fund fiscal year {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/group-fund-fiscal-years/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertGroupFundFiscalYear(JObject groupFundFiscalYear)
        {
            var s = Stopwatch.StartNew();
            if (groupFundFiscalYear["id"] == null) groupFundFiscalYear["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting group fund fiscal year {0}", groupFundFiscalYear["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/group-fund-fiscal-years";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = groupFundFiscalYear.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateGroupFundFiscalYear(JObject groupFundFiscalYear)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating group fund fiscal year {groupFundFiscalYear["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/group-fund-fiscal-years/{groupFundFiscalYear["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = groupFundFiscalYear.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteGroupFundFiscalYear(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting group fund fiscal year {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/group-fund-fiscal-years/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountHoldings(string where = null)
        {
            Holdings(out var i, take: 0);
            return i;
        }

        public JObject[] Holdings(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying holdings");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/holdings-storage/holdings{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Holdings(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying holdings");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-storage/holdings{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetHolding(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting holding {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-storage/holdings/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertHolding(JObject holding)
        {
            var s = Stopwatch.StartNew();
            if (holding["id"] == null) holding["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting holding {0}", holding["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-storage/holdings";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = holding.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertHoldings(IEnumerable<JObject> holdings)
        {
            var s = Stopwatch.StartNew();
            foreach (var h in holdings) if (h["id"] == null) h["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting holdings {string.Join(", ", holdings.Select(h => h["id"]))}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-storage/batch/synchronous";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = new JObject(new JProperty("holdingsRecords", new JArray(holdings))).ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers?.ContentType?.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateHolding(JObject holding)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating holding {holding["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-storage/holdings/{holding["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = holding.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteHolding(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting holding {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-storage/holdings/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountHoldingNoteTypes(string where = null)
        {
            HoldingNoteTypes(out var i, take: 0);
            return i;
        }

        public JObject[] HoldingNoteTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying holding note types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/holdings-note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> HoldingNoteTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying holding note types");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetHoldingNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting holding note type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertHoldingNoteType(JObject holdingNoteType)
        {
            var s = Stopwatch.StartNew();
            if (holdingNoteType["id"] == null) holdingNoteType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting holding note type {0}", holdingNoteType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-note-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = holdingNoteType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateHoldingNoteType(JObject holdingNoteType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating holding note type {holdingNoteType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-note-types/{holdingNoteType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = holdingNoteType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteHoldingNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting holding note type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountHoldingTypes(string where = null)
        {
            HoldingTypes(out var i, take: 0);
            return i;
        }

        public JObject[] HoldingTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying holding types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/holdings-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> HoldingTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying holding types");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetHoldingType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting holding type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertHoldingType(JObject holdingType)
        {
            var s = Stopwatch.StartNew();
            if (holdingType["id"] == null) holdingType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting holding type {0}", holdingType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = holdingType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateHoldingType(JObject holdingType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating holding type {holdingType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-types/{holdingType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = holdingType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteHoldingType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting holding type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetHridSetting()
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting hrid setting");
            AuthenticateIfNecessary();
            var url = $"{Url}/hrid-settings-storage/hrid-settings";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateHridSetting(JObject hridSetting)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating hrid setting {hridSetting["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/hrid-settings-storage/hrid-settings";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = hridSetting.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountIdTypes(string where = null)
        {
            IdTypes(out var i, take: 0);
            return i;
        }

        public JObject[] IdTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying id types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/identifier-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> IdTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying id types");
            AuthenticateIfNecessary();
            var url = $"{Url}/identifier-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetIdType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting id type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/identifier-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertIdType(JObject idType)
        {
            var s = Stopwatch.StartNew();
            if (idType["id"] == null) idType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting id type {0}", idType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/identifier-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = idType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateIdType(JObject idType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating id type {idType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/identifier-types/{idType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = idType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteIdType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting id type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/identifier-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountIllPolicies(string where = null)
        {
            IllPolicies(out var i, take: 0);
            return i;
        }

        public JObject[] IllPolicies(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying ill policies");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/ill-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> IllPolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying ill policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/ill-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetIllPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting ill policy {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/ill-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertIllPolicy(JObject illPolicy)
        {
            var s = Stopwatch.StartNew();
            if (illPolicy["id"] == null) illPolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting ill policy {0}", illPolicy["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/ill-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = illPolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateIllPolicy(JObject illPolicy)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating ill policy {illPolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/ill-policies/{illPolicy["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = illPolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteIllPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting ill policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/ill-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountInstances(string where = null)
        {
            Instances(out var i, take: 0);
            return i;
        }

        public JObject[] Instances(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instances");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/instance-storage/instances{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Instances(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instances");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instances{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetInstance(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting instance {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instances/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInstance(JObject instance)
        {
            var s = Stopwatch.StartNew();
            if (instance["id"] == null) instance["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting instance {0}", instance["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instances";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instance.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInstances(IEnumerable<JObject> instances)
        {
            var s = Stopwatch.StartNew();
            foreach (var i in instances) if (i["id"] == null) i["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting instances {string.Join(", ", instances.Select(i => i["id"]))}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/batch/synchronous";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = new JObject(new JProperty("instances", new JArray(instances))).ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers?.ContentType?.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateInstance(JObject instance)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating instance {instance["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instances/{instance["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instance.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteInstance(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instances/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountInstanceFormats(string where = null)
        {
            InstanceFormats(out var i, take: 0);
            return i;
        }

        public JObject[] InstanceFormats(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance formats");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/instance-formats{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> InstanceFormats(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance formats");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-formats{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetInstanceFormat(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting instance format {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-formats/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInstanceFormat(JObject instanceFormat)
        {
            var s = Stopwatch.StartNew();
            if (instanceFormat["id"] == null) instanceFormat["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting instance format {0}", instanceFormat["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-formats";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceFormat.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateInstanceFormat(JObject instanceFormat)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating instance format {instanceFormat["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-formats/{instanceFormat["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceFormat.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteInstanceFormat(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance format {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-formats/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountInstanceNoteTypes(string where = null)
        {
            InstanceNoteTypes(out var i, take: 0);
            return i;
        }

        public JObject[] InstanceNoteTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance note types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/instance-note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> InstanceNoteTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance note types");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetInstanceNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting instance note type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInstanceNoteType(JObject instanceNoteType)
        {
            var s = Stopwatch.StartNew();
            if (instanceNoteType["id"] == null) instanceNoteType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting instance note type {0}", instanceNoteType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-note-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceNoteType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateInstanceNoteType(JObject instanceNoteType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating instance note type {instanceNoteType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-note-types/{instanceNoteType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceNoteType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteInstanceNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance note type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountInstanceRelationships(string where = null)
        {
            InstanceRelationships(out var i, take: 0);
            return i;
        }

        public JObject[] InstanceRelationships(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance relationships");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/instance-storage/instance-relationships{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> InstanceRelationships(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance relationships");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instance-relationships{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetInstanceRelationship(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting instance relationship {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instance-relationships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInstanceRelationship(JObject instanceRelationship)
        {
            var s = Stopwatch.StartNew();
            if (instanceRelationship["id"] == null) instanceRelationship["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting instance relationship {0}", instanceRelationship["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instance-relationships";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceRelationship.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateInstanceRelationship(JObject instanceRelationship)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating instance relationship {instanceRelationship["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instance-relationships/{instanceRelationship["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceRelationship.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteInstanceRelationship(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance relationship {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instance-relationships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountInstanceRelationshipTypes(string where = null)
        {
            InstanceRelationshipTypes(out var i, take: 0);
            return i;
        }

        public JObject[] InstanceRelationshipTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance relationship types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/instance-relationship-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> InstanceRelationshipTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance relationship types");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-relationship-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetInstanceRelationshipType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting instance relationship type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-relationship-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInstanceRelationshipType(JObject instanceRelationshipType)
        {
            var s = Stopwatch.StartNew();
            if (instanceRelationshipType["id"] == null) instanceRelationshipType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting instance relationship type {0}", instanceRelationshipType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-relationship-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceRelationshipType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateInstanceRelationshipType(JObject instanceRelationshipType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating instance relationship type {instanceRelationshipType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-relationship-types/{instanceRelationshipType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceRelationshipType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteInstanceRelationshipType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance relationship type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-relationship-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountInstanceStatuses(string where = null)
        {
            InstanceStatuses(out var i, take: 0);
            return i;
        }

        public JObject[] InstanceStatuses(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance statuses");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/instance-statuses{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> InstanceStatuses(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance statuses");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-statuses{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetInstanceStatus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting instance status {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-statuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInstanceStatus(JObject instanceStatus)
        {
            var s = Stopwatch.StartNew();
            if (instanceStatus["id"] == null) instanceStatus["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting instance status {0}", instanceStatus["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-statuses";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceStatus.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateInstanceStatus(JObject instanceStatus)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating instance status {instanceStatus["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-statuses/{instanceStatus["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceStatus.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteInstanceStatus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance status {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-statuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountInstanceTypes(string where = null)
        {
            InstanceTypes(out var i, take: 0);
            return i;
        }

        public JObject[] InstanceTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/instance-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> InstanceTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance types");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetInstanceType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting instance type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInstanceType(JObject instanceType)
        {
            var s = Stopwatch.StartNew();
            if (instanceType["id"] == null) instanceType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting instance type {0}", instanceType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateInstanceType(JObject instanceType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating instance type {instanceType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-types/{instanceType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteInstanceType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountInstitutions(string where = null)
        {
            Institutions(out var i, take: 0);
            return i;
        }

        public JObject[] Institutions(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying institutions");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/location-units/institutions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Institutions(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying institutions");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/institutions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetInstitution(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting institution {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/institutions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInstitution(JObject institution)
        {
            var s = Stopwatch.StartNew();
            if (institution["id"] == null) institution["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting institution {0}", institution["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/institutions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = institution.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateInstitution(JObject institution)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating institution {institution["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/institutions/{institution["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = institution.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteInstitution(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting institution {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/institutions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountInterfaces(string where = null)
        {
            Interfaces(out var i, take: 0);
            return i;
        }

        public JObject[] Interfaces(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying interfaces");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/organizations-storage/interfaces{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Interfaces(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying interfaces");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/interfaces{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetInterface(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting interface {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/interfaces/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInterface(JObject @interface)
        {
            var s = Stopwatch.StartNew();
            if (@interface["id"] == null) @interface["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting interface {0}", @interface["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/interfaces";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = @interface.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateInterface(JObject @interface)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating interface {@interface["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/interfaces/{@interface["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = @interface.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteInterface(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting interface {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/interfaces/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountInvoices(string where = null)
        {
            Invoices(out var i, take: 0);
            return i;
        }

        public JObject[] Invoices(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying invoices");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/invoice-storage/invoices{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Invoices(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying invoices");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoices{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetInvoice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting invoice {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoices/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInvoice(JObject invoice)
        {
            var s = Stopwatch.StartNew();
            if (invoice["id"] == null) invoice["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting invoice {0}", invoice["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoices";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = invoice.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateInvoice(JObject invoice)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating invoice {invoice["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoices/{invoice["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = invoice.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteInvoice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting invoice {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoices/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountInvoiceItems(string where = null)
        {
            InvoiceItems(out var i, take: 0);
            return i;
        }

        public JObject[] InvoiceItems(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying invoice items");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/invoice-storage/invoice-lines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> InvoiceItems(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying invoice items");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoice-lines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetInvoiceItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting invoice item {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoice-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertInvoiceItem(JObject invoiceItem)
        {
            var s = Stopwatch.StartNew();
            if (invoiceItem["id"] == null) invoiceItem["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting invoice item {0}", invoiceItem["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoice-lines";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = invoiceItem.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateInvoiceItem(JObject invoiceItem)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating invoice item {invoiceItem["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoice-lines/{invoiceItem["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = invoiceItem.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteInvoiceItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting invoice item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoice-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountItems(string where = null)
        {
            Items(out var i, take: 0);
            return i;
        }

        public JObject[] Items(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying items");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/item-storage/items{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Items(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying items");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-storage/items{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting item {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/item-storage/items/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertItem(JObject item)
        {
            var s = Stopwatch.StartNew();
            if (item["id"] == null) item["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting item {0}", item["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/item-storage/items";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = item.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertItems(IEnumerable<JObject> items)
        {
            var s = Stopwatch.StartNew();
            foreach (var i in items) if (i["id"] == null) i["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting items {string.Join(", ", items.Select(i => i["id"]))}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-storage/batch/synchronous";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = new JObject(new JProperty("items", new JArray(items))).ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers?.ContentType?.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateItem(JObject item)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating item {item["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-storage/items/{item["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = item.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-storage/items/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountItemDamagedStatuses(string where = null)
        {
            ItemDamagedStatuses(out var i, take: 0);
            return i;
        }

        public JObject[] ItemDamagedStatuses(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying item damaged statuses");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/item-damaged-statuses{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> ItemDamagedStatuses(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying item damaged statuses");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-damaged-statuses{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetItemDamagedStatus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting item damaged status {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/item-damaged-statuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertItemDamagedStatus(JObject itemDamagedStatus)
        {
            var s = Stopwatch.StartNew();
            if (itemDamagedStatus["id"] == null) itemDamagedStatus["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting item damaged status {0}", itemDamagedStatus["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/item-damaged-statuses";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = itemDamagedStatus.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateItemDamagedStatus(JObject itemDamagedStatus)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating item damaged status {itemDamagedStatus["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-damaged-statuses/{itemDamagedStatus["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = itemDamagedStatus.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteItemDamagedStatus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting item damaged status {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-damaged-statuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountItemNoteTypes(string where = null)
        {
            ItemNoteTypes(out var i, take: 0);
            return i;
        }

        public JObject[] ItemNoteTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying item note types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/item-note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> ItemNoteTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying item note types");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetItemNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting item note type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/item-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertItemNoteType(JObject itemNoteType)
        {
            var s = Stopwatch.StartNew();
            if (itemNoteType["id"] == null) itemNoteType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting item note type {0}", itemNoteType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/item-note-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = itemNoteType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateItemNoteType(JObject itemNoteType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating item note type {itemNoteType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-note-types/{itemNoteType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = itemNoteType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteItemNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting item note type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountLedgers(string where = null)
        {
            Ledgers(out var i, take: 0);
            return i;
        }

        public JObject[] Ledgers(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying ledgers");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/finance-storage/ledgers{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Ledgers(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying ledgers");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledgers{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetLedger(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting ledger {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledgers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertLedger(JObject ledger)
        {
            var s = Stopwatch.StartNew();
            if (ledger["id"] == null) ledger["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting ledger {0}", ledger["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledgers";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = ledger.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateLedger(JObject ledger)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating ledger {ledger["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledgers/{ledger["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = ledger.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteLedger(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting ledger {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledgers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountLibraries(string where = null)
        {
            Libraries(out var i, take: 0);
            return i;
        }

        public JObject[] Libraries(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying libraries");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/location-units/libraries{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Libraries(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying libraries");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/libraries{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetLibrary(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting library {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/libraries/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertLibrary(JObject library)
        {
            var s = Stopwatch.StartNew();
            if (library["id"] == null) library["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting library {0}", library["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/libraries";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = library.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateLibrary(JObject library)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating library {library["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/libraries/{library["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = library.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteLibrary(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting library {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/libraries/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountLoans(string where = null)
        {
            Loans(out var i, take: 0);
            return i;
        }

        public JObject[] Loans(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying loans");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/loan-storage/loans{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Loans(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying loans");
            AuthenticateIfNecessary();
            skip = skip ?? 0;
            take = take ?? int.MaxValue;
            orderBy = orderBy ?? "id";
            while (take > 0)
            {
                var url = $"{Url}/loan-storage/loans{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={Math.Min(take.Value, 10000)}";
                traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
                var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
                if (hrm.StatusCode != HttpStatusCode.OK)
                {
                    var s2 = hrm.Content.ReadAsStringAsync().Result;
                    if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
                }
                using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
                using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
                {
                    if (!jtr.Read()) throw new InvalidDataException();
                    while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                    var js = new JsonSerializer();
                    var i = 0;
                    while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                    {
                        var jo = (JObject)js.Deserialize(jtr);
                        traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                        ++i;
                        yield return jo;
                    }
                    if (i < Math.Min(take.Value, 10000)) break;
                    skip += i;
                    take -= i;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetLoan(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting loan {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-storage/loans/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertLoan(JObject loan)
        {
            var s = Stopwatch.StartNew();
            if (loan["id"] == null) loan["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting loan {0}", loan["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-storage/loans";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = loan.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateLoan(JObject loan)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating loan {loan["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-storage/loans/{loan["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = loan.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteLoan(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting loan {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-storage/loans/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountLoanPolicies(string where = null)
        {
            LoanPolicies(out var i, take: 0);
            return i;
        }

        public JObject[] LoanPolicies(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying loan policies");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/loan-policy-storage/loan-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> LoanPolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying loan policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-policy-storage/loan-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetLoanPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting loan policy {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-policy-storage/loan-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertLoanPolicy(JObject loanPolicy)
        {
            var s = Stopwatch.StartNew();
            if (loanPolicy["id"] == null) loanPolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting loan policy {0}", loanPolicy["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-policy-storage/loan-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = loanPolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateLoanPolicy(JObject loanPolicy)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating loan policy {loanPolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-policy-storage/loan-policies/{loanPolicy["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = loanPolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteLoanPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting loan policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-policy-storage/loan-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountLoanTypes(string where = null)
        {
            LoanTypes(out var i, take: 0);
            return i;
        }

        public JObject[] LoanTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying loan types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/loan-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> LoanTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying loan types");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetLoanType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting loan type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertLoanType(JObject loanType)
        {
            var s = Stopwatch.StartNew();
            if (loanType["id"] == null) loanType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting loan type {0}", loanType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = loanType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateLoanType(JObject loanType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating loan type {loanType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-types/{loanType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = loanType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteLoanType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting loan type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountLocations(string where = null)
        {
            Locations(out var i, take: 0);
            return i;
        }

        public JObject[] Locations(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying locations");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/locations{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Locations(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying locations");
            AuthenticateIfNecessary();
            var url = $"{Url}/locations{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetLocation(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting location {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/locations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertLocation(JObject location)
        {
            var s = Stopwatch.StartNew();
            if (location["id"] == null) location["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting location {0}", location["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/locations";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = location.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateLocation(JObject location)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating location {location["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/locations/{location["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = location.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteLocation(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting location {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/locations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject InsertLogin(JObject login)
        {
            var s = Stopwatch.StartNew();
            if (login["id"] == null) login["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting login {0}", login["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/authn/credentials";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = login.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public int CountLostItemFeePolicies(string where = null)
        {
            LostItemFeePolicies(out var i, take: 0);
            return i;
        }

        public JObject[] LostItemFeePolicies(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying lost item fee policies");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/lost-item-fees-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> LostItemFeePolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying lost item fee policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/lost-item-fees-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetLostItemFeePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting lost item fee policy {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/lost-item-fees-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertLostItemFeePolicy(JObject lostItemFeePolicy)
        {
            var s = Stopwatch.StartNew();
            if (lostItemFeePolicy["id"] == null) lostItemFeePolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting lost item fee policy {0}", lostItemFeePolicy["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/lost-item-fees-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = lostItemFeePolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateLostItemFeePolicy(JObject lostItemFeePolicy)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating lost item fee policy {lostItemFeePolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/lost-item-fees-policies/{lostItemFeePolicy["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = lostItemFeePolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteLostItemFeePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting lost item fee policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/lost-item-fees-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountMaterialTypes(string where = null)
        {
            MaterialTypes(out var i, take: 0);
            return i;
        }

        public JObject[] MaterialTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying material types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/material-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> MaterialTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying material types");
            AuthenticateIfNecessary();
            var url = $"{Url}/material-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetMaterialType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting material type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/material-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertMaterialType(JObject materialType)
        {
            var s = Stopwatch.StartNew();
            if (materialType["id"] == null) materialType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting material type {0}", materialType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/material-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = materialType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateMaterialType(JObject materialType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating material type {materialType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/material-types/{materialType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = materialType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteMaterialType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting material type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/material-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountModeOfIssuances(string where = null)
        {
            ModeOfIssuances(out var i, take: 0);
            return i;
        }

        public JObject[] ModeOfIssuances(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying mode of issuances");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/modes-of-issuance{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> ModeOfIssuances(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying mode of issuances");
            AuthenticateIfNecessary();
            var url = $"{Url}/modes-of-issuance{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetModeOfIssuance(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting mode of issuance {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/modes-of-issuance/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertModeOfIssuance(JObject modeOfIssuance)
        {
            var s = Stopwatch.StartNew();
            if (modeOfIssuance["id"] == null) modeOfIssuance["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting mode of issuance {0}", modeOfIssuance["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/modes-of-issuance";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = modeOfIssuance.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateModeOfIssuance(JObject modeOfIssuance)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating mode of issuance {modeOfIssuance["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/modes-of-issuance/{modeOfIssuance["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = modeOfIssuance.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteModeOfIssuance(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting mode of issuance {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/modes-of-issuance/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountNatureOfContentTerms(string where = null)
        {
            NatureOfContentTerms(out var i, take: 0);
            return i;
        }

        public JObject[] NatureOfContentTerms(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying nature of content terms");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/nature-of-content-terms{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> NatureOfContentTerms(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying nature of content terms");
            AuthenticateIfNecessary();
            var url = $"{Url}/nature-of-content-terms{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetNatureOfContentTerm(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting nature of content term {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/nature-of-content-terms/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertNatureOfContentTerm(JObject natureOfContentTerm)
        {
            var s = Stopwatch.StartNew();
            if (natureOfContentTerm["id"] == null) natureOfContentTerm["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting nature of content term {0}", natureOfContentTerm["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/nature-of-content-terms";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = natureOfContentTerm.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateNatureOfContentTerm(JObject natureOfContentTerm)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating nature of content term {natureOfContentTerm["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/nature-of-content-terms/{natureOfContentTerm["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = natureOfContentTerm.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteNatureOfContentTerm(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting nature of content term {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/nature-of-content-terms/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountNotes(string where = null)
        {
            Notes(out var i, take: 0);
            return i;
        }

        public JObject[] Notes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying notes");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/notes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Notes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying notes");
            AuthenticateIfNecessary();
            var url = $"{Url}/notes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetNote(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting note {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/notes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertNote(JObject note)
        {
            var s = Stopwatch.StartNew();
            if (note["id"] == null) note["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting note {0}", note["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/notes";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = note.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateNote(JObject note)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating note {note["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/notes/{note["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = note.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteNote(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting note {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/notes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountNoteTypes(string where = null)
        {
            NoteTypes(out var i, take: 0);
            return i;
        }

        public JObject[] NoteTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying note types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> NoteTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying note types");
            AuthenticateIfNecessary();
            var url = $"{Url}/note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting note type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertNoteType(JObject noteType)
        {
            var s = Stopwatch.StartNew();
            if (noteType["id"] == null) noteType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting note type {0}", noteType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/note-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = noteType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateNoteType(JObject noteType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating note type {noteType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/note-types/{noteType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = noteType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting note type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountOrders(string where = null)
        {
            Orders(out var i, take: 0);
            return i;
        }

        public JObject[] Orders(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying orders");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/orders-storage/purchase-orders{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Orders(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying orders");
            AuthenticateIfNecessary();
            skip = skip ?? 0;
            take = take ?? int.MaxValue;
            orderBy = orderBy ?? "id";
            while (take > 0)
            {
                var url = $"{Url}/orders-storage/purchase-orders{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={Math.Min(take.Value, 10000)}";
                traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
                var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
                if (hrm.StatusCode != HttpStatusCode.OK)
                {
                    var s2 = hrm.Content.ReadAsStringAsync().Result;
                    if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
                }
                using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
                using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
                {
                    if (!jtr.Read()) throw new InvalidDataException();
                    while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                    var js = new JsonSerializer();
                    var i = 0;
                    while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                    {
                        var jo = (JObject)js.Deserialize(jtr);
                        traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                        ++i;
                        yield return jo;
                    }
                    if (i < Math.Min(take.Value, 10000)) break;
                    skip += i;
                    take -= i;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetOrder(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting order {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/purchase-orders/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertOrder(JObject order)
        {
            var s = Stopwatch.StartNew();
            if (order["id"] == null) order["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting order {0}", order["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/purchase-orders";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = order.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateOrder(JObject order)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating order {order["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/purchase-orders/{order["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = order.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteOrder(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting order {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/purchase-orders/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountOrderInvoices(string where = null)
        {
            OrderInvoices(out var i, take: 0);
            return i;
        }

        public JObject[] OrderInvoices(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying order invoices");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/orders-storage/order-invoice-relns{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> OrderInvoices(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying order invoices");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-invoice-relns{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetOrderInvoice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting order invoice {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-invoice-relns/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertOrderInvoice(JObject orderInvoice)
        {
            var s = Stopwatch.StartNew();
            if (orderInvoice["id"] == null) orderInvoice["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting order invoice {0}", orderInvoice["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-invoice-relns";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = orderInvoice.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateOrderInvoice(JObject orderInvoice)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating order invoice {orderInvoice["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-invoice-relns/{orderInvoice["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = orderInvoice.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteOrderInvoice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting order invoice {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-invoice-relns/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountOrderItems(string where = null)
        {
            OrderItems(out var i, take: 0);
            return i;
        }

        public JObject[] OrderItems(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying order items");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/orders-storage/po-lines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> OrderItems(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying order items");
            AuthenticateIfNecessary();
            skip = skip ?? 0;
            take = take ?? int.MaxValue;
            orderBy = orderBy ?? "id";
            while (take > 0)
            {
                var url = $"{Url}/orders-storage/po-lines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={Math.Min(take.Value, 10000)}";
                traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
                var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
                if (hrm.StatusCode != HttpStatusCode.OK)
                {
                    var s2 = hrm.Content.ReadAsStringAsync().Result;
                    if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
                }
                using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
                using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
                {
                    if (!jtr.Read()) throw new InvalidDataException();
                    while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                    var js = new JsonSerializer();
                    var i = 0;
                    while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                    {
                        var jo = (JObject)js.Deserialize(jtr);
                        traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                        ++i;
                        yield return jo;
                    }
                    if (i < Math.Min(take.Value, 10000)) break;
                    skip += i;
                    take -= i;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetOrderItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting order item {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/po-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertOrderItem(JObject orderItem)
        {
            var s = Stopwatch.StartNew();
            if (orderItem["id"] == null) orderItem["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting order item {0}", orderItem["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/po-lines";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = orderItem.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateOrderItem(JObject orderItem)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating order item {orderItem["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/po-lines/{orderItem["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = orderItem.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteOrderItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting order item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/po-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountOrderTemplates(string where = null)
        {
            OrderTemplates(out var i, take: 0);
            return i;
        }

        public JObject[] OrderTemplates(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying order templates");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/orders-storage/order-templates{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> OrderTemplates(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying order templates");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-templates{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetOrderTemplate(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting order template {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-templates/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertOrderTemplate(JObject orderTemplate)
        {
            var s = Stopwatch.StartNew();
            if (orderTemplate["id"] == null) orderTemplate["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting order template {0}", orderTemplate["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-templates";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = orderTemplate.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateOrderTemplate(JObject orderTemplate)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating order template {orderTemplate["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-templates/{orderTemplate["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = orderTemplate.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteOrderTemplate(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting order template {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-templates/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountOrganizations(string where = null)
        {
            Organizations(out var i, take: 0);
            return i;
        }

        public JObject[] Organizations(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying organizations");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/organizations-storage/organizations{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Organizations(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying organizations");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/organizations{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetOrganization(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting organization {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/organizations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertOrganization(JObject organization)
        {
            var s = Stopwatch.StartNew();
            if (organization["id"] == null) organization["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting organization {0}", organization["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/organizations";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = organization.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateOrganization(JObject organization)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating organization {organization["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/organizations/{organization["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = organization.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteOrganization(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting organization {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/organizations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountOverdueFinePolicies(string where = null)
        {
            OverdueFinePolicies(out var i, take: 0);
            return i;
        }

        public JObject[] OverdueFinePolicies(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying overdue fine policies");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/overdue-fines-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> OverdueFinePolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying overdue fine policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/overdue-fines-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetOverdueFinePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting overdue fine policy {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/overdue-fines-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertOverdueFinePolicy(JObject overdueFinePolicy)
        {
            var s = Stopwatch.StartNew();
            if (overdueFinePolicy["id"] == null) overdueFinePolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting overdue fine policy {0}", overdueFinePolicy["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/overdue-fines-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = overdueFinePolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateOverdueFinePolicy(JObject overdueFinePolicy)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating overdue fine policy {overdueFinePolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/overdue-fines-policies/{overdueFinePolicy["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = overdueFinePolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteOverdueFinePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting overdue fine policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/overdue-fines-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountOwners(string where = null)
        {
            Owners(out var i, take: 0);
            return i;
        }

        public JObject[] Owners(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying owners");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/owners{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Owners(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying owners");
            AuthenticateIfNecessary();
            var url = $"{Url}/owners{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetOwner(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting owner {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/owners/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertOwner(JObject owner)
        {
            var s = Stopwatch.StartNew();
            if (owner["id"] == null) owner["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting owner {0}", owner["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/owners";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = owner.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateOwner(JObject owner)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating owner {owner["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/owners/{owner["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = owner.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteOwner(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting owner {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/owners/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountPatronActionSessions(string where = null)
        {
            PatronActionSessions(out var i, take: 0);
            return i;
        }

        public JObject[] PatronActionSessions(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying patron action sessions");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/patron-action-session-storage/patron-action-sessions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> PatronActionSessions(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying patron action sessions");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-action-session-storage/patron-action-sessions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetPatronActionSession(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting patron action session {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-action-session-storage/patron-action-sessions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertPatronActionSession(JObject patronActionSession)
        {
            var s = Stopwatch.StartNew();
            if (patronActionSession["id"] == null) patronActionSession["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting patron action session {0}", patronActionSession["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-action-session-storage/patron-action-sessions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = patronActionSession.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdatePatronActionSession(JObject patronActionSession)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating patron action session {patronActionSession["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-action-session-storage/patron-action-sessions/{patronActionSession["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = patronActionSession.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeletePatronActionSession(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting patron action session {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-action-session-storage/patron-action-sessions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountPatronNoticePolicies(string where = null)
        {
            PatronNoticePolicies(out var i, take: 0);
            return i;
        }

        public JObject[] PatronNoticePolicies(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying patron notice policies");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/patron-notice-policy-storage/patron-notice-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> PatronNoticePolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying patron notice policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-notice-policy-storage/patron-notice-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetPatronNoticePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting patron notice policy {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-notice-policy-storage/patron-notice-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertPatronNoticePolicy(JObject patronNoticePolicy)
        {
            var s = Stopwatch.StartNew();
            if (patronNoticePolicy["id"] == null) patronNoticePolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting patron notice policy {0}", patronNoticePolicy["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-notice-policy-storage/patron-notice-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = patronNoticePolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdatePatronNoticePolicy(JObject patronNoticePolicy)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating patron notice policy {patronNoticePolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-notice-policy-storage/patron-notice-policies/{patronNoticePolicy["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = patronNoticePolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeletePatronNoticePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting patron notice policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-notice-policy-storage/patron-notice-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountPayments(string where = null)
        {
            Payments(out var i, take: 0);
            return i;
        }

        public JObject[] Payments(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying payments");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/feefineactions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Payments(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying payments");
            AuthenticateIfNecessary();
            skip = skip ?? 0;
            take = take ?? int.MaxValue;
            orderBy = orderBy ?? "id";
            while (take > 0)
            {
                var url = $"{Url}/feefineactions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={Math.Min(take.Value, 10000)}";
                traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
                var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
                if (hrm.StatusCode != HttpStatusCode.OK)
                {
                    var s2 = hrm.Content.ReadAsStringAsync().Result;
                    if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
                }
                using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
                using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
                {
                    if (!jtr.Read()) throw new InvalidDataException();
                    while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                    var js = new JsonSerializer();
                    var i = 0;
                    while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                    {
                        var jo = (JObject)js.Deserialize(jtr);
                        traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                        ++i;
                        yield return jo;
                    }
                    if (i < Math.Min(take.Value, 10000)) break;
                    skip += i;
                    take -= i;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetPayment(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting payment {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/feefineactions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertPayment(JObject payment)
        {
            var s = Stopwatch.StartNew();
            if (payment["id"] == null) payment["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting payment {0}", payment["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/feefineactions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = payment.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdatePayment(JObject payment)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating payment {payment["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefineactions/{payment["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = payment.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeletePayment(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting payment {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefineactions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountPaymentMethods(string where = null)
        {
            PaymentMethods(out var i, take: 0);
            return i;
        }

        public JObject[] PaymentMethods(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying payment methods");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/payments{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> PaymentMethods(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying payment methods");
            AuthenticateIfNecessary();
            var url = $"{Url}/payments{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetPaymentMethod(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting payment method {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/payments/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertPaymentMethod(JObject paymentMethod)
        {
            var s = Stopwatch.StartNew();
            if (paymentMethod["id"] == null) paymentMethod["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting payment method {0}", paymentMethod["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/payments";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = paymentMethod.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdatePaymentMethod(JObject paymentMethod)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating payment method {paymentMethod["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/payments/{paymentMethod["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = paymentMethod.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeletePaymentMethod(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting payment method {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/payments/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountPermissions(string where = null)
        {
            Permissions(out var i, take: 0);
            return i;
        }

        public JObject[] Permissions(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying permissions");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/perms/permissions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}start={skip + 1}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}length={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Permissions(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying permissions");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/permissions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}start={skip + 1}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}length={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetPermission(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting permission {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/permissions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertPermission(JObject permission)
        {
            var s = Stopwatch.StartNew();
            if (permission["id"] == null) permission["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting permission {0}", permission["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/permissions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = permission.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdatePermission(JObject permission)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating permission {permission["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/permissions/{permission["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = permission.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeletePermission(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting permission {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/permissions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountPermissionsUsers(string where = null)
        {
            PermissionsUsers(out var i, take: 0);
            return i;
        }

        public JObject[] PermissionsUsers(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying permissions users");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/perms/users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}start={skip + 1}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}length={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> PermissionsUsers(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying permissions users");
            AuthenticateIfNecessary();
            skip = skip ?? 0;
            take = take ?? int.MaxValue;
            orderBy = orderBy ?? "id";
            while (take > 0)
            {
                var url = $"{Url}/perms/users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}start={skip + 1}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}length={Math.Min(take.Value, 10000)}";
                traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
                var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
                if (hrm.StatusCode != HttpStatusCode.OK)
                {
                    var s2 = hrm.Content.ReadAsStringAsync().Result;
                    if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
                }
                using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
                using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
                {
                    if (!jtr.Read()) throw new InvalidDataException();
                    while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                    var js = new JsonSerializer();
                    var i = 0;
                    while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                    {
                        var jo = (JObject)js.Deserialize(jtr);
                        traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                        ++i;
                        yield return jo;
                    }
                    if (i < Math.Min(take.Value, 10000)) break;
                    skip += i;
                    take -= i;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetPermissionsUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting permissions user {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertPermissionsUser(JObject permissionsUser)
        {
            var s = Stopwatch.StartNew();
            if (permissionsUser["id"] == null) permissionsUser["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting permissions user {0}", permissionsUser["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/users";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = permissionsUser.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdatePermissionsUser(JObject permissionsUser)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating permissions user {permissionsUser["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/users/{permissionsUser["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = permissionsUser.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeletePermissionsUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting permissions user {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountPieces(string where = null)
        {
            Pieces(out var i, take: 0);
            return i;
        }

        public JObject[] Pieces(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying pieces");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/orders-storage/pieces{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Pieces(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying pieces");
            AuthenticateIfNecessary();
            skip = skip ?? 0;
            take = take ?? int.MaxValue;
            orderBy = orderBy ?? "id";
            while (take > 0)
            {
                var url = $"{Url}/orders-storage/pieces{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={Math.Min(take.Value, 10000)}";
                traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
                var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
                if (hrm.StatusCode != HttpStatusCode.OK)
                {
                    var s2 = hrm.Content.ReadAsStringAsync().Result;
                    if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
                }
                using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
                using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
                {
                    if (!jtr.Read()) throw new InvalidDataException();
                    while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                    var js = new JsonSerializer();
                    var i = 0;
                    while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                    {
                        var jo = (JObject)js.Deserialize(jtr);
                        traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                        ++i;
                        yield return jo;
                    }
                    if (i < Math.Min(take.Value, 10000)) break;
                    skip += i;
                    take -= i;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetPiece(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting piece {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/pieces/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertPiece(JObject piece)
        {
            var s = Stopwatch.StartNew();
            if (piece["id"] == null) piece["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting piece {0}", piece["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/pieces";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = piece.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdatePiece(JObject piece)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating piece {piece["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/pieces/{piece["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = piece.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeletePiece(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting piece {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/pieces/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountPrecedingSucceedingTitles(string where = null)
        {
            PrecedingSucceedingTitles(out var i, take: 0);
            return i;
        }

        public JObject[] PrecedingSucceedingTitles(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying preceding succeeding titles");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/preceding-succeeding-titles{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> PrecedingSucceedingTitles(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying preceding succeeding titles");
            AuthenticateIfNecessary();
            skip = skip ?? 0;
            take = take ?? int.MaxValue;
            orderBy = orderBy ?? "id";
            while (take > 0)
            {
                var url = $"{Url}/preceding-succeeding-titles{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={Math.Min(take.Value, 10000)}";
                traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
                var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
                if (hrm.StatusCode != HttpStatusCode.OK)
                {
                    var s2 = hrm.Content.ReadAsStringAsync().Result;
                    if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
                }
                using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
                using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
                {
                    if (!jtr.Read()) throw new InvalidDataException();
                    while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                    var js = new JsonSerializer();
                    var i = 0;
                    while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                    {
                        var jo = (JObject)js.Deserialize(jtr);
                        traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                        ++i;
                        yield return jo;
                    }
                    if (i < Math.Min(take.Value, 10000)) break;
                    skip += i;
                    take -= i;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetPrecedingSucceedingTitle(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting preceding succeeding title {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/preceding-succeeding-titles/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertPrecedingSucceedingTitle(JObject precedingSucceedingTitle)
        {
            var s = Stopwatch.StartNew();
            if (precedingSucceedingTitle["id"] == null) precedingSucceedingTitle["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting preceding succeeding title {0}", precedingSucceedingTitle["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/preceding-succeeding-titles";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = precedingSucceedingTitle.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdatePrecedingSucceedingTitle(JObject precedingSucceedingTitle)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating preceding succeeding title {precedingSucceedingTitle["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/preceding-succeeding-titles/{precedingSucceedingTitle["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = precedingSucceedingTitle.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeletePrecedingSucceedingTitle(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting preceding succeeding title {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/preceding-succeeding-titles/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountPrefixes(string where = null)
        {
            Prefixes(out var i, take: 0);
            return i;
        }

        public JObject[] Prefixes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying prefixes");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/orders-storage/configuration/prefixes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Prefixes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying prefixes");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/prefixes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetPrefix(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting prefix {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/prefixes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertPrefix(JObject prefix)
        {
            var s = Stopwatch.StartNew();
            if (prefix["id"] == null) prefix["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting prefix {0}", prefix["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/prefixes";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = prefix.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdatePrefix(JObject prefix)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating prefix {prefix["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/prefixes/{prefix["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = prefix.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeletePrefix(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting prefix {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/prefixes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountProxies(string where = null)
        {
            Proxies(out var i, take: 0);
            return i;
        }

        public JObject[] Proxies(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying proxies");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/proxiesfor{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Proxies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying proxies");
            AuthenticateIfNecessary();
            var url = $"{Url}/proxiesfor{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetProxy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting proxy {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/proxiesfor/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertProxy(JObject proxy)
        {
            var s = Stopwatch.StartNew();
            if (proxy["id"] == null) proxy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting proxy {0}", proxy["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/proxiesfor";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = proxy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateProxy(JObject proxy)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating proxy {proxy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/proxiesfor/{proxy["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = proxy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteProxy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting proxy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/proxiesfor/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountRecords(string where = null)
        {
            Records(out var i, take: 0);
            return i;
        }

        public JObject[] Records(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying records");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/source-storage/records{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Records(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying records");
            AuthenticateIfNecessary();
            skip = skip ?? 0;
            take = take ?? int.MaxValue;
            orderBy = orderBy ?? "id";
            while (take > 0)
            {
                var url = $"{Url}/source-storage/records{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={Math.Min(take.Value, 5000)}";
                traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
                var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
                if (hrm.StatusCode != HttpStatusCode.OK)
                {
                    var s2 = hrm.Content.ReadAsStringAsync().Result;
                    if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
                }
                using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
                using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
                {
                    if (!jtr.Read()) throw new InvalidDataException();
                    while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                    var js = new JsonSerializer();
                    var i = 0;
                    while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                    {
                        var jo = (JObject)js.Deserialize(jtr);
                        traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                        ++i;
                        yield return jo;
                    }
                    if (i < Math.Min(take.Value, 5000)) break;
                    skip += i;
                    take -= i;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetRecord(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting record {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/records/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertRecord(JObject record)
        {
            var s = Stopwatch.StartNew();
            if (record["id"] == null) record["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting record {0}", record["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/records";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = record.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertRecords(IEnumerable<JObject> records)
        {
            var s = Stopwatch.StartNew();
            foreach (var r in records) if (r["id"] == null) r["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting records {string.Join(", ", records.Select(r => r["id"]))}");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/batch/records";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = new JObject(new JProperty("records", new JArray(records)), new JProperty("totalRecords", records.Count())).ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers?.ContentType?.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateRecord(JObject record)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating record {record["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/records/{record["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = record.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteRecord(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting record {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/records/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountRefundReasons(string where = null)
        {
            RefundReasons(out var i, take: 0);
            return i;
        }

        public JObject[] RefundReasons(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying refund reasons");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/refunds{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> RefundReasons(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying refund reasons");
            AuthenticateIfNecessary();
            var url = $"{Url}/refunds{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetRefundReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting refund reason {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/refunds/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertRefundReason(JObject refundReason)
        {
            var s = Stopwatch.StartNew();
            if (refundReason["id"] == null) refundReason["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting refund reason {0}", refundReason["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/refunds";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = refundReason.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateRefundReason(JObject refundReason)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating refund reason {refundReason["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/refunds/{refundReason["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = refundReason.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteRefundReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting refund reason {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/refunds/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountReportingCodes(string where = null)
        {
            ReportingCodes(out var i, take: 0);
            return i;
        }

        public JObject[] ReportingCodes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying reporting codes");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/orders-storage/reporting-codes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> ReportingCodes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying reporting codes");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/reporting-codes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetReportingCode(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting reporting code {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/reporting-codes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertReportingCode(JObject reportingCode)
        {
            var s = Stopwatch.StartNew();
            if (reportingCode["id"] == null) reportingCode["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting reporting code {0}", reportingCode["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/reporting-codes";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = reportingCode.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateReportingCode(JObject reportingCode)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating reporting code {reportingCode["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/reporting-codes/{reportingCode["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = reportingCode.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteReportingCode(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting reporting code {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/reporting-codes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountRequests(string where = null)
        {
            Requests(out var i, take: 0);
            return i;
        }

        public JObject[] Requests(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying requests");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/request-storage/requests{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Requests(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying requests");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-storage/requests{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetRequest(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting request {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/request-storage/requests/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertRequest(JObject request)
        {
            var s = Stopwatch.StartNew();
            if (request["id"] == null) request["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting request {0}", request["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/request-storage/requests";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = request.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateRequest(JObject request)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating request {request["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-storage/requests/{request["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = request.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteRequest(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting request {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-storage/requests/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountRequestPolicies(string where = null)
        {
            RequestPolicies(out var i, take: 0);
            return i;
        }

        public JObject[] RequestPolicies(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying request policies");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/request-policy-storage/request-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> RequestPolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying request policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-policy-storage/request-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetRequestPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting request policy {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/request-policy-storage/request-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertRequestPolicy(JObject requestPolicy)
        {
            var s = Stopwatch.StartNew();
            if (requestPolicy["id"] == null) requestPolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting request policy {0}", requestPolicy["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/request-policy-storage/request-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = requestPolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateRequestPolicy(JObject requestPolicy)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating request policy {requestPolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-policy-storage/request-policies/{requestPolicy["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = requestPolicy.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteRequestPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting request policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-policy-storage/request-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountScheduledNotices(string where = null)
        {
            ScheduledNotices(out var i, take: 0);
            return i;
        }

        public JObject[] ScheduledNotices(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying scheduled notices");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/scheduled-notice-storage/scheduled-notices{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> ScheduledNotices(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying scheduled notices");
            AuthenticateIfNecessary();
            var url = $"{Url}/scheduled-notice-storage/scheduled-notices{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetScheduledNotice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting scheduled notice {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/scheduled-notice-storage/scheduled-notices/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertScheduledNotice(JObject scheduledNotice)
        {
            var s = Stopwatch.StartNew();
            if (scheduledNotice["id"] == null) scheduledNotice["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting scheduled notice {0}", scheduledNotice["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/scheduled-notice-storage/scheduled-notices";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = scheduledNotice.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateScheduledNotice(JObject scheduledNotice)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating scheduled notice {scheduledNotice["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/scheduled-notice-storage/scheduled-notices/{scheduledNotice["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = scheduledNotice.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteScheduledNotice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting scheduled notice {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/scheduled-notice-storage/scheduled-notices/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountServicePoints(string where = null)
        {
            ServicePoints(out var i, take: 0);
            return i;
        }

        public JObject[] ServicePoints(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying service points");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/service-points{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> ServicePoints(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying service points");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetServicePoint(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting service point {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertServicePoint(JObject servicePoint)
        {
            var s = Stopwatch.StartNew();
            if (servicePoint["id"] == null) servicePoint["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting service point {0}", servicePoint["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = servicePoint.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateServicePoint(JObject servicePoint)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating service point {servicePoint["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points/{servicePoint["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = servicePoint.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteServicePoint(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting service point {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountServicePointUsers(string where = null)
        {
            ServicePointUsers(out var i, take: 0);
            return i;
        }

        public JObject[] ServicePointUsers(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying service point users");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/service-points-users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> ServicePointUsers(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying service point users");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points-users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetServicePointUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting service point user {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points-users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertServicePointUser(JObject servicePointUser)
        {
            var s = Stopwatch.StartNew();
            if (servicePointUser["id"] == null) servicePointUser["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting service point user {0}", servicePointUser["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points-users";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = servicePointUser.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateServicePointUser(JObject servicePointUser)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating service point user {servicePointUser["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points-users/{servicePointUser["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = servicePointUser.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteServicePointUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting service point user {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points-users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountSnapshots(string where = null)
        {
            Snapshots(out var i, take: 0);
            return i;
        }

        public JObject[] Snapshots(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying snapshots");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/source-storage/snapshots{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Snapshots(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying snapshots");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/snapshots{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetSnapshot(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting snapshot {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/snapshots/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertSnapshot(JObject snapshot)
        {
            var s = Stopwatch.StartNew();
            if (snapshot["jobExecutionId"] == null) snapshot["jobExecutionId"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting snapshot {0}", snapshot["jobExecutionId"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/snapshots";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = snapshot.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateSnapshot(JObject snapshot)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating snapshot {snapshot["jobExecutionId"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/snapshots/{snapshot["jobExecutionId"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = snapshot.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteSnapshot(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting snapshot {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/snapshots/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountStaffSlips(string where = null)
        {
            StaffSlips(out var i, take: 0);
            return i;
        }

        public JObject[] StaffSlips(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying staff slips");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/staff-slips-storage/staff-slips{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> StaffSlips(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying staff slips");
            AuthenticateIfNecessary();
            var url = $"{Url}/staff-slips-storage/staff-slips{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetStaffSlip(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting staff slip {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/staff-slips-storage/staff-slips/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertStaffSlip(JObject staffSlip)
        {
            var s = Stopwatch.StartNew();
            if (staffSlip["id"] == null) staffSlip["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting staff slip {0}", staffSlip["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/staff-slips-storage/staff-slips";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = staffSlip.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateStaffSlip(JObject staffSlip)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating staff slip {staffSlip["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/staff-slips-storage/staff-slips/{staffSlip["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = staffSlip.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteStaffSlip(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting staff slip {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/staff-slips-storage/staff-slips/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountStatisticalCodes(string where = null)
        {
            StatisticalCodes(out var i, take: 0);
            return i;
        }

        public JObject[] StatisticalCodes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying statistical codes");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/statistical-codes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> StatisticalCodes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying statistical codes");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-codes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetStatisticalCode(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting statistical code {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-codes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertStatisticalCode(JObject statisticalCode)
        {
            var s = Stopwatch.StartNew();
            if (statisticalCode["id"] == null) statisticalCode["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting statistical code {0}", statisticalCode["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-codes";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = statisticalCode.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateStatisticalCode(JObject statisticalCode)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating statistical code {statisticalCode["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-codes/{statisticalCode["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = statisticalCode.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteStatisticalCode(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting statistical code {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-codes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountStatisticalCodeTypes(string where = null)
        {
            StatisticalCodeTypes(out var i, take: 0);
            return i;
        }

        public JObject[] StatisticalCodeTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying statistical code types");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/statistical-code-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> StatisticalCodeTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying statistical code types");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-code-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetStatisticalCodeType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting statistical code type {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-code-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertStatisticalCodeType(JObject statisticalCodeType)
        {
            var s = Stopwatch.StartNew();
            if (statisticalCodeType["id"] == null) statisticalCodeType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting statistical code type {0}", statisticalCodeType["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-code-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = statisticalCodeType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateStatisticalCodeType(JObject statisticalCodeType)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating statistical code type {statisticalCodeType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-code-types/{statisticalCodeType["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = statisticalCodeType.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteStatisticalCodeType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting statistical code type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-code-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountSuffixes(string where = null)
        {
            Suffixes(out var i, take: 0);
            return i;
        }

        public JObject[] Suffixes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying suffixes");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/orders-storage/configuration/suffixes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Suffixes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying suffixes");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/suffixes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetSuffix(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting suffix {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/suffixes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertSuffix(JObject suffix)
        {
            var s = Stopwatch.StartNew();
            if (suffix["id"] == null) suffix["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting suffix {0}", suffix["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/suffixes";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = suffix.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateSuffix(JObject suffix)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating suffix {suffix["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/suffixes/{suffix["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = suffix.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteSuffix(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting suffix {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/suffixes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountTags(string where = null)
        {
            Tags(out var i, take: 0);
            return i;
        }

        public JObject[] Tags(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying tags");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/tags{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Tags(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying tags");
            AuthenticateIfNecessary();
            var url = $"{Url}/tags{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetTag(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting tag {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/tags/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertTag(JObject tag)
        {
            var s = Stopwatch.StartNew();
            if (tag["id"] == null) tag["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting tag {0}", tag["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/tags";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = tag.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateTag(JObject tag)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating tag {tag["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/tags/{tag["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = tag.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteTag(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting tag {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/tags/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountTemplates(string where = null)
        {
            Templates(out var i, take: 0);
            return i;
        }

        public JObject[] Templates(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying templates");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/templates{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Templates(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying templates");
            AuthenticateIfNecessary();
            var url = $"{Url}/templates{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetTemplate(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting template {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/templates/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertTemplate(JObject template)
        {
            var s = Stopwatch.StartNew();
            if (template["id"] == null) template["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting template {0}", template["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/templates";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = template.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateTemplate(JObject template)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating template {template["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/templates/{template["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = template.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteTemplate(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting template {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/templates/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountTitles(string where = null)
        {
            Titles(out var i, take: 0);
            return i;
        }

        public JObject[] Titles(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying titles");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/orders-storage/titles{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Titles(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying titles");
            AuthenticateIfNecessary();
            skip = skip ?? 0;
            take = take ?? int.MaxValue;
            orderBy = orderBy ?? "id";
            while (take > 0)
            {
                var url = $"{Url}/orders-storage/titles{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={Math.Min(take.Value, 10000)}";
                traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
                var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
                if (hrm.StatusCode != HttpStatusCode.OK)
                {
                    var s2 = hrm.Content.ReadAsStringAsync().Result;
                    if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
                }
                using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
                using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
                {
                    if (!jtr.Read()) throw new InvalidDataException();
                    while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                    var js = new JsonSerializer();
                    var i = 0;
                    while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                    {
                        var jo = (JObject)js.Deserialize(jtr);
                        traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                        ++i;
                        yield return jo;
                    }
                    if (i < Math.Min(take.Value, 10000)) break;
                    skip += i;
                    take -= i;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetTitle(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting title {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/titles/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertTitle(JObject title)
        {
            var s = Stopwatch.StartNew();
            if (title["id"] == null) title["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting title {0}", title["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/titles";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = title.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateTitle(JObject title)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating title {title["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/titles/{title["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = title.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteTitle(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting title {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/titles/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountTransactions(string where = null)
        {
            Transactions(out var i, take: 0);
            return i;
        }

        public JObject[] Transactions(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying transactions");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/finance-storage/transactions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Transactions(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying transactions");
            AuthenticateIfNecessary();
            skip = skip ?? 0;
            take = take ?? int.MaxValue;
            orderBy = orderBy ?? "id";
            while (take > 0)
            {
                var url = $"{Url}/finance-storage/transactions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={Math.Min(take.Value, 10000)}";
                traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
                var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
                if (hrm.StatusCode != HttpStatusCode.OK)
                {
                    var s2 = hrm.Content.ReadAsStringAsync().Result;
                    if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
                }
                using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
                using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
                {
                    if (!jtr.Read()) throw new InvalidDataException();
                    while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                    var js = new JsonSerializer();
                    var i = 0;
                    while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                    {
                        var jo = (JObject)js.Deserialize(jtr);
                        traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                        ++i;
                        yield return jo;
                    }
                    if (i < Math.Min(take.Value, 10000)) break;
                    skip += i;
                    take -= i;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetTransaction(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting transaction {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/transactions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertTransaction(JObject transaction)
        {
            var s = Stopwatch.StartNew();
            if (transaction["id"] == null) transaction["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting transaction {0}", transaction["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/transactions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = transaction.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateTransaction(JObject transaction)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating transaction {transaction["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/transactions/{transaction["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = transaction.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteTransaction(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting transaction {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/transactions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountTransferAccounts(string where = null)
        {
            TransferAccounts(out var i, take: 0);
            return i;
        }

        public JObject[] TransferAccounts(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying transfer accounts");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/transfers{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> TransferAccounts(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying transfer accounts");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfers{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetTransferAccount(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting transfer account {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/transfers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertTransferAccount(JObject transferAccount)
        {
            var s = Stopwatch.StartNew();
            if (transferAccount["id"] == null) transferAccount["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting transfer account {0}", transferAccount["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/transfers";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = transferAccount.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateTransferAccount(JObject transferAccount)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating transfer account {transferAccount["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfers/{transferAccount["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = transferAccount.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteTransferAccount(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting transfer account {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountTransferCriterias(string where = null)
        {
            TransferCriterias(out var i, take: 0);
            return i;
        }

        public JObject[] TransferCriterias(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying transfer criterias");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/transfer-criterias{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> TransferCriterias(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying transfer criterias");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfer-criterias{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetTransferCriteria(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting transfer criteria {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/transfer-criterias/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertTransferCriteria(JObject transferCriteria)
        {
            var s = Stopwatch.StartNew();
            if (transferCriteria["id"] == null) transferCriteria["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting transfer criteria {0}", transferCriteria["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/transfer-criterias";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = transferCriteria.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateTransferCriteria(JObject transferCriteria)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating transfer criteria {transferCriteria["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfer-criterias/{transferCriteria["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = transferCriteria.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteTransferCriteria(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting transfer criteria {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfer-criterias/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountUsers(string where = null)
        {
            Users(out var i, take: 0);
            return i;
        }

        public JObject[] Users(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying users");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Users(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying users");
            AuthenticateIfNecessary();
            var url = $"{Url}/users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting user {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertUser(JObject user)
        {
            var s = Stopwatch.StartNew();
            if (user["id"] == null) user["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting user {0}", user["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/users";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = user.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateUser(JObject user)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating user {user["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/users/{user["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = user.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject ImportUsers(IEnumerable<JObject> users, string source = null, bool disable = true, bool merge = true)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Importing users {string.Join(", ", users.Select(u => u["id"]))}");
            AuthenticateIfNecessary();
            var url = $"{Url}/user-import";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = new JObject(new JProperty("users", new JArray(users)), new JProperty("totalRecords", users.Count()), new JProperty("deactivateMissingUsers", disable), new JProperty("updateOnlyPresentFields", merge), new JProperty("sourceType", source)).ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers?.ContentType?.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void DeleteUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting user {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountUserAcquisitionsUnits(string where = null)
        {
            UserAcquisitionsUnits(out var i, take: 0);
            return i;
        }

        public JObject[] UserAcquisitionsUnits(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying user acquisitions units");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/acquisitions-units-storage/memberships{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> UserAcquisitionsUnits(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying user acquisitions units");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/memberships{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetUserAcquisitionsUnit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting user acquisitions unit {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/memberships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertUserAcquisitionsUnit(JObject userAcquisitionsUnit)
        {
            var s = Stopwatch.StartNew();
            if (userAcquisitionsUnit["id"] == null) userAcquisitionsUnit["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting user acquisitions unit {0}", userAcquisitionsUnit["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/memberships";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = userAcquisitionsUnit.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateUserAcquisitionsUnit(JObject userAcquisitionsUnit)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating user acquisitions unit {userAcquisitionsUnit["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/memberships/{userAcquisitionsUnit["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = userAcquisitionsUnit.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteUserAcquisitionsUnit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting user acquisitions unit {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/memberships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountUserRequestPreferences(string where = null)
        {
            UserRequestPreferences(out var i, take: 0);
            return i;
        }

        public JObject[] UserRequestPreferences(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying user request preferences");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/request-preference-storage/request-preference{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> UserRequestPreferences(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying user request preferences");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-preference-storage/request-preference{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetUserRequestPreference(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting user request preference {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/request-preference-storage/request-preference/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertUserRequestPreference(JObject userRequestPreference)
        {
            var s = Stopwatch.StartNew();
            if (userRequestPreference["id"] == null) userRequestPreference["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting user request preference {0}", userRequestPreference["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/request-preference-storage/request-preference";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = userRequestPreference.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateUserRequestPreference(JObject userRequestPreference)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating user request preference {userRequestPreference["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-preference-storage/request-preference/{userRequestPreference["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = userRequestPreference.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteUserRequestPreference(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting user request preference {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-preference-storage/request-preference/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountVouchers(string where = null)
        {
            Vouchers(out var i, take: 0);
            return i;
        }

        public JObject[] Vouchers(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying vouchers");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/voucher-storage/vouchers{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> Vouchers(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying vouchers");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/vouchers{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetVoucher(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting voucher {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/vouchers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertVoucher(JObject voucher)
        {
            var s = Stopwatch.StartNew();
            if (voucher["id"] == null) voucher["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting voucher {0}", voucher["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/vouchers";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = voucher.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateVoucher(JObject voucher)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating voucher {voucher["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/vouchers/{voucher["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = voucher.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteVoucher(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting voucher {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/vouchers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountVoucherItems(string where = null)
        {
            VoucherItems(out var i, take: 0);
            return i;
        }

        public JObject[] VoucherItems(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying voucher items");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/voucher-storage/voucher-lines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> VoucherItems(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying voucher items");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/voucher-lines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetVoucherItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting voucher item {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/voucher-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertVoucherItem(JObject voucherItem)
        {
            var s = Stopwatch.StartNew();
            if (voucherItem["id"] == null) voucherItem["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting voucher item {0}", voucherItem["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/voucher-lines";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = voucherItem.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateVoucherItem(JObject voucherItem)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating voucher item {voucherItem["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/voucher-lines/{voucherItem["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = voucherItem.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteVoucherItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting voucher item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/voucher-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public int CountWaiveReasons(string where = null)
        {
            WaiveReasons(out var i, take: 0);
            return i;
        }

        public JObject[] WaiveReasons(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying waive reasons");
            AuthenticateIfNecessary();
            if ((skip != null || take != null) && take != 0) orderBy = orderBy ?? "id";
            var url = $"{Url}/waives{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            count = (int)jo["totalRecords"];
            return jo.Properties().SkipWhile(jp => jp.Name == "totalRecords").First().Value.Cast<JObject>().ToArray();
        }

        public IEnumerable<JObject> WaiveReasons(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying waive reasons");
            AuthenticateIfNecessary();
            var url = $"{Url}/waives{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", hrm.Headers);
            if (hrm.StatusCode != HttpStatusCode.OK)
            {
                var s2 = hrm.Content.ReadAsStringAsync().Result;
                if (hrm.Content.Headers.ContentType.MediaType == "application/json") s2 = JObject.Parse(s2).ToString();
                traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
                throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            }
            using (var sr = new StreamReader(hrm.Content.ReadAsStreamAsync().Result))
            using (var jtr = new JsonTextReader(sr) { SupportMultipleContent = true })
            {
                if (!jtr.Read()) throw new InvalidDataException();
                while (jtr.Read() && jtr.TokenType != JsonToken.StartArray) ;
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject GetWaiveReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Getting waive reason {0}", id);
            if (id == null) return null;
            AuthenticateIfNecessary();
            var url = $"{Url}/waives/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK && hrm.StatusCode != HttpStatusCode.NotFound) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public JObject InsertWaiveReason(JObject waiveReason)
        {
            var s = Stopwatch.StartNew();
            if (waiveReason["id"] == null) waiveReason["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Inserting waive reason {0}", waiveReason["id"]);
            AuthenticateIfNecessary();
            var url = $"{Url}/waives";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = waiveReason.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public void UpdateWaiveReason(JObject waiveReason)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating waive reason {waiveReason["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/waives/{waiveReason["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = waiveReason.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public void DeleteWaiveReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting waive reason {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/waives/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
        }

        public JObject[] Modules()
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying modules");
            AuthenticateIfNecessary();
            var url = $"{Url}/_/proxy/tenants/{Tenant}/modules";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", httpClient.DefaultRequestHeaders);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var ja = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JArray.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return ja.Cast<JObject>().ToArray();
        }

        public JObject InsertObject(JObject jObject, string url)
        {
            var s = Stopwatch.StartNew();
            if (jObject["id"] == null) jObject["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting object {0}", jObject["id"]);
            AuthenticateIfNecessary();
            url = $"{Url}/{url}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = jObject.ToString(formatting);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", httpClient.DefaultRequestHeaders, s2);
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}{1}", hrm.Headers, s2);
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", s.Elapsed);
            return jo;
        }

        public static string EncodeCql(string value)
        {
            if (value == null) return null;
            value = value.Replace("\"", "\\\"");
            value = value.Replace("?", "\\?");
            value = value.Replace("*", "\\*");
            value = value.Replace("^", "\\^");
            return value;
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }

    public class ArrayJsonConverter<T, T2> : JsonConverter<T> where T : ICollection<T2>, new() where T2 : new()
    {
        private string name;

        public ArrayJsonConverter(string name) => this.name = name;

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var l = new T();
            foreach (var jt in JToken.Load(reader))
            {
                var t2 = new T2();
                t2.GetType().GetProperty(name).SetValue(t2, jt.ToObject(t2.GetType().GetProperty(name).PropertyType));
                l.Add(t2);
            }
            return l;
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonPathJsonConverter<T> : JsonConverter<T> where T : new()
    {
        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var t = new T();
            foreach (var pi in objectType.GetProperties().Where(pi => pi.CanWrite && !pi.GetCustomAttributes(true).OfType<JsonIgnoreAttribute>().Any() && (objectType.GetCustomAttributes(true).OfType<JsonObjectAttribute>().SingleOrDefault()?.MemberSerialization != MemberSerialization.OptIn || pi.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().Any())))
            {
                var jt = jo.SelectToken(pi.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().FirstOrDefault()?.PropertyName ?? (serializer.ContractResolver is DefaultContractResolver resolver ? resolver.GetResolvedPropertyName(pi.Name) : pi.Name));
                if (jt != null && jt.Type != JTokenType.Null)
                {
                    var jca = pi.GetCustomAttributes(true).OfType<JsonConverterAttribute>().SingleOrDefault();
                    pi.SetValue(t, jca != null ? jca.ConverterType.GetMethods().Where(mi2 => mi2.Name == "ReadJson").First().Invoke(Activator.CreateInstance(jca.ConverterType, jca.ConverterParameters), new object[] { jt.CreateReader(), objectType, existingValue, hasExistingValue, serializer }) : jt.ToObject(pi.PropertyType, serializer), null);
                }
            }
            return t;
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
