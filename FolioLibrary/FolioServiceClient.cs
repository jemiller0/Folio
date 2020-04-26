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

namespace FolioLibrary
{
    public class FolioServiceClient : IDisposable
    {
        private string accessToken = ConfigurationManager.AppSettings["accessToken"];
        private Formatting formatting = traceSource.Switch.Level == SourceLevels.Verbose ? Formatting.Indented : Formatting.None;
        private HttpClient httpClient = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };
        private readonly static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented };
        public string Password { get; set; } = ConfigurationManager.AppSettings["password"];
        public string Tenant { get; set; } = ConfigurationManager.AppSettings["tenant"];
        public readonly static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.Information);
        public string Username { get; set; } = ConfigurationManager.AppSettings["username"];
        public string Url { get; set; } = ConfigurationManager.AppSettings["url"];

        public FolioServiceClient()
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-tenant", Tenant);
            if (accessToken != null) httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-token", accessToken);
        }

        private void AuthenticateIfNecessary()
        {
            if (accessToken == null) Authenticate();
        }

        public void Authenticate()
        {
            var s2 = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Authenticating");
            var url = $"{Url}/authn/login";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s = JsonConvert.SerializeObject(new { Username, Password }, jsonSerializerSettings);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s);
            var hrm = httpClient.PostAsync(url, new StringContent(s, Encoding.UTF8, "application/json")).Result;
            s = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s);
            if (hrm.StatusCode != HttpStatusCode.Created)
                if (s == "Bad credentials")
                    throw new InvalidCredentialException(s);
                else
                    throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s}");
            accessToken = hrm.Headers.GetValues("x-okapi-token").Single();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-okapi-token", accessToken);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2.Elapsed.ToString());
        }

        public IEnumerable<JObject> AcquisitionsUnits(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying acquisitions units");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/units{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetAcquisitionsUnit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting acquisitions unit {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/units/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertAcquisitionsUnit(JObject acquisitionsUnit)
        {
            var s = Stopwatch.StartNew();
            if (acquisitionsUnit["id"] == null) acquisitionsUnit["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting acquisitions unit {acquisitionsUnit["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/units";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = acquisitionsUnit.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteAcquisitionsUnit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting acquisitions unit {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/units/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> AddressTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying address types");
            AuthenticateIfNecessary();
            var url = $"{Url}/addresstypes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetAddressType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting address type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/addresstypes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertAddressType(JObject addressType)
        {
            var s = Stopwatch.StartNew();
            if (addressType["id"] == null) addressType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting address type {addressType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/addresstypes";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = addressType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteAddressType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting address type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/addresstypes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Alerts(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying alerts");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/alerts{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetAlert(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting alert {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/alerts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertAlert(JObject alert)
        {
            var s = Stopwatch.StartNew();
            if (alert["id"] == null) alert["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting alert {alert["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/alerts";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = alert.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteAlert(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting alert {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/alerts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> AlternativeTitleTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying alternative title types");
            AuthenticateIfNecessary();
            var url = $"{Url}/alternative-title-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetAlternativeTitleType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting alternative title type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/alternative-title-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertAlternativeTitleType(JObject alternativeTitleType)
        {
            var s = Stopwatch.StartNew();
            if (alternativeTitleType["id"] == null) alternativeTitleType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting alternative title type {alternativeTitleType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/alternative-title-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = alternativeTitleType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteAlternativeTitleType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting alternative title type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/alternative-title-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> BatchGroups(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying batch groups");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-group-storage/batch-groups{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetBatchGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting batch group {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-group-storage/batch-groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertBatchGroup(JObject batchGroup)
        {
            var s = Stopwatch.StartNew();
            if (batchGroup["id"] == null) batchGroup["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting batch group {batchGroup["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-group-storage/batch-groups";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = batchGroup.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteBatchGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting batch group {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-group-storage/batch-groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> BatchVoucherExports(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying batch voucher exports");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/batch-voucher-exports{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetBatchVoucherExport(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting batch voucher export {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/batch-voucher-exports/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertBatchVoucherExport(JObject batchVoucherExport)
        {
            var s = Stopwatch.StartNew();
            if (batchVoucherExport["id"] == null) batchVoucherExport["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting batch voucher export {batchVoucherExport["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/batch-voucher-exports";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = batchVoucherExport.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteBatchVoucherExport(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting batch voucher export {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/batch-voucher-exports/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> BatchVoucherExportConfigs(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying batch voucher export configs");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/export-configurations{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetBatchVoucherExportConfig(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting batch voucher export config {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/export-configurations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertBatchVoucherExportConfig(JObject batchVoucherExportConfig)
        {
            var s = Stopwatch.StartNew();
            if (batchVoucherExportConfig["id"] == null) batchVoucherExportConfig["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting batch voucher export config {batchVoucherExportConfig["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/export-configurations";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = batchVoucherExportConfig.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteBatchVoucherExportConfig(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting batch voucher export config {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/batch-voucher-storage/export-configurations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Blocks(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying blocks");
            AuthenticateIfNecessary();
            var url = $"{Url}/manualblocks{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetBlock(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting block {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/manualblocks/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertBlock(JObject block)
        {
            var s = Stopwatch.StartNew();
            if (block["id"] == null) block["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting block {block["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/manualblocks";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = block.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteBlock(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting block {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/manualblocks/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Budgets(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying budgets");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/budgets{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetBudget(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting budget {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/budgets/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertBudget(JObject budget)
        {
            var s = Stopwatch.StartNew();
            if (budget["id"] == null) budget["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting budget {budget["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/budgets";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = budget.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteBudget(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting budget {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/budgets/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> CallNumberTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying call number types");
            AuthenticateIfNecessary();
            var url = $"{Url}/call-number-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetCallNumberType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting call number type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/call-number-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertCallNumberType(JObject callNumberType)
        {
            var s = Stopwatch.StartNew();
            if (callNumberType["id"] == null) callNumberType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting call number type {callNumberType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/call-number-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = callNumberType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteCallNumberType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting call number type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/call-number-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Campuses(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying campuses");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/campuses{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetCampus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting campus {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/campuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertCampus(JObject campus)
        {
            var s = Stopwatch.StartNew();
            if (campus["id"] == null) campus["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting campus {campus["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/campuses";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = campus.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteCampus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting campus {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/campuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> CancellationReasons(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying cancellation reasons");
            AuthenticateIfNecessary();
            var url = $"{Url}/cancellation-reason-storage/cancellation-reasons{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetCancellationReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting cancellation reason {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/cancellation-reason-storage/cancellation-reasons/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertCancellationReason(JObject cancellationReason)
        {
            var s = Stopwatch.StartNew();
            if (cancellationReason["id"] == null) cancellationReason["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting cancellation reason {cancellationReason["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/cancellation-reason-storage/cancellation-reasons";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = cancellationReason.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteCancellationReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting cancellation reason {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/cancellation-reason-storage/cancellation-reasons/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Categories(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying categories");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/categories{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetCategory(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting category {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/categories/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertCategory(JObject category)
        {
            var s = Stopwatch.StartNew();
            if (category["id"] == null) category["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting category {category["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/categories";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = category.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteCategory(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting category {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/categories/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> CheckIns(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying check ins");
            AuthenticateIfNecessary();
            var url = $"{Url}/check-in-storage/check-ins{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetCheckIn(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting check in {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/check-in-storage/check-ins/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertCheckIn(JObject checkIn)
        {
            var s = Stopwatch.StartNew();
            if (checkIn["id"] == null) checkIn["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting check in {checkIn["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/check-in-storage/check-ins";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = checkIn.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteCheckIn(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting check in {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/check-in-storage/check-ins/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ClassificationTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying classification types");
            AuthenticateIfNecessary();
            var url = $"{Url}/classification-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetClassificationType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting classification type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/classification-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertClassificationType(JObject classificationType)
        {
            var s = Stopwatch.StartNew();
            if (classificationType["id"] == null) classificationType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting classification type {classificationType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/classification-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = classificationType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteClassificationType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting classification type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/classification-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Comments(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying comments");
            AuthenticateIfNecessary();
            var url = $"{Url}/comments{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetComment(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting comment {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/comments/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertComment(JObject comment)
        {
            var s = Stopwatch.StartNew();
            if (comment["id"] == null) comment["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting comment {comment["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/comments";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = comment.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteComment(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting comment {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/comments/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Configurations(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying configurations");
            AuthenticateIfNecessary();
            var url = $"{Url}/configurations/entries{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetConfiguration(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting configuration {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/configurations/entries/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertConfiguration(JObject configuration)
        {
            var s = Stopwatch.StartNew();
            if (configuration["id"] == null) configuration["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting configuration {configuration["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/configurations/entries";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = configuration.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteConfiguration(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting configuration {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/configurations/entries/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Contacts(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying contacts");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/contacts{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetContact(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting contact {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/contacts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertContact(JObject contact)
        {
            var s = Stopwatch.StartNew();
            if (contact["id"] == null) contact["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting contact {contact["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/contacts";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = contact.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteContact(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting contact {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/contacts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ContributorNameTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying contributor name types");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-name-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetContributorNameType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting contributor name type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-name-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertContributorNameType(JObject contributorNameType)
        {
            var s = Stopwatch.StartNew();
            if (contributorNameType["id"] == null) contributorNameType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting contributor name type {contributorNameType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-name-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = contributorNameType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteContributorNameType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting contributor name type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-name-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ContributorTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying contributor types");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetContributorType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting contributor type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertContributorType(JObject contributorType)
        {
            var s = Stopwatch.StartNew();
            if (contributorType["id"] == null) contributorType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting contributor type {contributorType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = contributorType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteContributorType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting contributor type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ElectronicAccessRelationships(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying electronic access relationships");
            AuthenticateIfNecessary();
            var url = $"{Url}/electronic-access-relationships{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetElectronicAccessRelationship(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting electronic access relationship {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/electronic-access-relationships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertElectronicAccessRelationship(JObject electronicAccessRelationship)
        {
            var s = Stopwatch.StartNew();
            if (electronicAccessRelationship["id"] == null) electronicAccessRelationship["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting electronic access relationship {electronicAccessRelationship["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/electronic-access-relationships";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = electronicAccessRelationship.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteElectronicAccessRelationship(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting electronic access relationship {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/electronic-access-relationships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Fees(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fees");
            AuthenticateIfNecessary();
            var url = $"{Url}/accounts{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetFee(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting fee {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/accounts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertFee(JObject fee)
        {
            var s = Stopwatch.StartNew();
            if (fee["id"] == null) fee["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting fee {fee["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/accounts";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fee.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteFee(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fee {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/accounts/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> FeeTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fee types");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetFeeType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting fee type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertFeeType(JObject feeType)
        {
            var s = Stopwatch.StartNew();
            if (feeType["id"] == null) feeType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting fee type {feeType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefines";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = feeType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteFeeType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fee type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> FinanceGroups(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying finance groups");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/groups{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetFinanceGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting finance group {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertFinanceGroup(JObject financeGroup)
        {
            var s = Stopwatch.StartNew();
            if (financeGroup["id"] == null) financeGroup["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting finance group {financeGroup["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/groups";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = financeGroup.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteFinanceGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting finance group {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> FiscalYears(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fiscal years");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fiscal-years{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetFiscalYear(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting fiscal year {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fiscal-years/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertFiscalYear(JObject fiscalYear)
        {
            var s = Stopwatch.StartNew();
            if (fiscalYear["id"] == null) fiscalYear["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting fiscal year {fiscalYear["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fiscal-years";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fiscalYear.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteFiscalYear(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fiscal year {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fiscal-years/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> FixedDueDateSchedules(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fixed due date schedules");
            AuthenticateIfNecessary();
            var url = $"{Url}/fixed-due-date-schedule-storage/fixed-due-date-schedules{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetFixedDueDateSchedule(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting fixed due date schedule {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/fixed-due-date-schedule-storage/fixed-due-date-schedules/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertFixedDueDateSchedule(JObject fixedDueDateSchedule)
        {
            var s = Stopwatch.StartNew();
            if (fixedDueDateSchedule["id"] == null) fixedDueDateSchedule["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting fixed due date schedule {fixedDueDateSchedule["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/fixed-due-date-schedule-storage/fixed-due-date-schedules";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fixedDueDateSchedule.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteFixedDueDateSchedule(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fixed due date schedule {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/fixed-due-date-schedule-storage/fixed-due-date-schedules/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Funds(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying funds");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/funds{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetFund(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting fund {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/funds/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertFund(JObject fund)
        {
            var s = Stopwatch.StartNew();
            if (fund["id"] == null) fund["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting fund {fund["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/funds";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fund.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteFund(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fund {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/funds/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> FundTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying fund types");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fund-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetFundType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting fund type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fund-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertFundType(JObject fundType)
        {
            var s = Stopwatch.StartNew();
            if (fundType["id"] == null) fundType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting fund type {fundType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fund-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = fundType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteFundType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting fund type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/fund-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Groups(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying groups");
            AuthenticateIfNecessary();
            var url = $"{Url}/groups{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting group {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertGroup(JObject group)
        {
            var s = Stopwatch.StartNew();
            if (group["id"] == null) group["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting group {group["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/groups";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = group.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteGroup(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting group {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/groups/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> GroupFundFiscalYears(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying group fund fiscal years");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/group-fund-fiscal-years{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetGroupFundFiscalYear(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting group fund fiscal year {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/group-fund-fiscal-years/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertGroupFundFiscalYear(JObject groupFundFiscalYear)
        {
            var s = Stopwatch.StartNew();
            if (groupFundFiscalYear["id"] == null) groupFundFiscalYear["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting group fund fiscal year {groupFundFiscalYear["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/group-fund-fiscal-years";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = groupFundFiscalYear.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteGroupFundFiscalYear(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting group fund fiscal year {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/group-fund-fiscal-years/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Holdings(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying holdings");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-storage/holdings{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read(); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetHolding(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting holding {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-storage/holdings/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertHolding(JObject holding)
        {
            var s = Stopwatch.StartNew();
            if (holding["id"] == null) holding["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting holding {holding["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-storage/holdings";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = holding.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers?.ContentType?.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteHolding(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting holding {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-storage/holdings/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> HoldingNoteTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying holding note types");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetHoldingNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting holding note type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertHoldingNoteType(JObject holdingNoteType)
        {
            var s = Stopwatch.StartNew();
            if (holdingNoteType["id"] == null) holdingNoteType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting holding note type {holdingNoteType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-note-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = holdingNoteType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteHoldingNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting holding note type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> HoldingTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying holding types");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetHoldingType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting holding type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertHoldingType(JObject holdingType)
        {
            var s = Stopwatch.StartNew();
            if (holdingType["id"] == null) holdingType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting holding type {holdingType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = holdingType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteHoldingType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting holding type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> IdTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying id types");
            AuthenticateIfNecessary();
            var url = $"{Url}/identifier-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetIdType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting id type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/identifier-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertIdType(JObject idType)
        {
            var s = Stopwatch.StartNew();
            if (idType["id"] == null) idType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting id type {idType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/identifier-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = idType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteIdType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting id type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/identifier-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> IllPolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying ill policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/ill-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetIllPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting ill policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/ill-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertIllPolicy(JObject illPolicy)
        {
            var s = Stopwatch.StartNew();
            if (illPolicy["id"] == null) illPolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting ill policy {illPolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/ill-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = illPolicy.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteIllPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting ill policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/ill-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Instances(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instances");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instances{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read(); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetInstance(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting instance {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instances/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertInstance(JObject instance)
        {
            var s = Stopwatch.StartNew();
            if (instance["id"] == null) instance["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting instance {instance["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instances";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instance.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers?.ContentType?.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteInstance(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instances/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> InstanceFormats(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance formats");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-formats{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetInstanceFormat(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting instance format {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-formats/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertInstanceFormat(JObject instanceFormat)
        {
            var s = Stopwatch.StartNew();
            if (instanceFormat["id"] == null) instanceFormat["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting instance format {instanceFormat["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-formats";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceFormat.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteInstanceFormat(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance format {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-formats/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> InstanceNoteTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance note types");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetInstanceNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting instance note type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertInstanceNoteType(JObject instanceNoteType)
        {
            var s = Stopwatch.StartNew();
            if (instanceNoteType["id"] == null) instanceNoteType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting instance note type {instanceNoteType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-note-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceNoteType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteInstanceNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance note type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> InstanceRelationships(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance relationships");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instance-relationships{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetInstanceRelationship(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting instance relationship {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instance-relationships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertInstanceRelationship(JObject instanceRelationship)
        {
            var s = Stopwatch.StartNew();
            if (instanceRelationship["id"] == null) instanceRelationship["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting instance relationship {instanceRelationship["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instance-relationships";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceRelationship.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteInstanceRelationship(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance relationship {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instance-relationships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> InstanceRelationshipTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance relationship types");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-relationship-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetInstanceRelationshipType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting instance relationship type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-relationship-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertInstanceRelationshipType(JObject instanceRelationshipType)
        {
            var s = Stopwatch.StartNew();
            if (instanceRelationshipType["id"] == null) instanceRelationshipType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting instance relationship type {instanceRelationshipType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-relationship-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceRelationshipType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteInstanceRelationshipType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance relationship type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-relationship-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> InstanceStatuses(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance statuses");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-statuses{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetInstanceStatus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting instance status {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-statuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertInstanceStatus(JObject instanceStatus)
        {
            var s = Stopwatch.StartNew();
            if (instanceStatus["id"] == null) instanceStatus["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting instance status {instanceStatus["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-statuses";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceStatus.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteInstanceStatus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance status {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-statuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> InstanceTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying instance types");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetInstanceType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting instance type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertInstanceType(JObject instanceType)
        {
            var s = Stopwatch.StartNew();
            if (instanceType["id"] == null) instanceType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting instance type {instanceType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = instanceType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteInstanceType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting instance type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Institutions(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying institutions");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/institutions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetInstitution(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting institution {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/institutions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertInstitution(JObject institution)
        {
            var s = Stopwatch.StartNew();
            if (institution["id"] == null) institution["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting institution {institution["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/institutions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = institution.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteInstitution(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting institution {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/institutions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Interfaces(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying interfaces");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/interfaces{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetInterface(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting interface {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/interfaces/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertInterface(JObject @interface)
        {
            var s = Stopwatch.StartNew();
            if (@interface["id"] == null) @interface["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting interface {@interface["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/interfaces";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = @interface.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteInterface(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting interface {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/interfaces/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Invoices(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying invoices");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoices{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetInvoice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting invoice {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoices/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertInvoice(JObject invoice)
        {
            var s = Stopwatch.StartNew();
            if (invoice["id"] == null) invoice["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting invoice {invoice["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoices";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = invoice.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteInvoice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting invoice {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoices/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> InvoiceItems(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying invoice items");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoice-lines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetInvoiceItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting invoice item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoice-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertInvoiceItem(JObject invoiceItem)
        {
            var s = Stopwatch.StartNew();
            if (invoiceItem["id"] == null) invoiceItem["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting invoice item {invoiceItem["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoice-lines";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = invoiceItem.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteInvoiceItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting invoice item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/invoice-storage/invoice-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Items(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying items");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-storage/items{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read(); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-storage/items/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertItem(JObject item)
        {
            var s = Stopwatch.StartNew();
            if (item["id"] == null) item["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting item {item["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-storage/items";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = item.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers?.ContentType?.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-storage/items/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ItemDamagedStatuses(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying item damaged statuses");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-damaged-statuses{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetItemDamagedStatus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting item damaged status {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-damaged-statuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertItemDamagedStatus(JObject itemDamagedStatus)
        {
            var s = Stopwatch.StartNew();
            if (itemDamagedStatus["id"] == null) itemDamagedStatus["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting item damaged status {itemDamagedStatus["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-damaged-statuses";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = itemDamagedStatus.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteItemDamagedStatus(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting item damaged status {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-damaged-statuses/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ItemNoteTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying item note types");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetItemNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting item note type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertItemNoteType(JObject itemNoteType)
        {
            var s = Stopwatch.StartNew();
            if (itemNoteType["id"] == null) itemNoteType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting item note type {itemNoteType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-note-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = itemNoteType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteItemNoteType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting item note type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-note-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Ledgers(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying ledgers");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledgers{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetLedger(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting ledger {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledgers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertLedger(JObject ledger)
        {
            var s = Stopwatch.StartNew();
            if (ledger["id"] == null) ledger["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting ledger {ledger["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledgers";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = ledger.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteLedger(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting ledger {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledgers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> LedgerFiscalYears(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying ledger fiscal years");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledger-fiscal-years{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetLedgerFiscalYear(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting ledger fiscal year {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledger-fiscal-years/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertLedgerFiscalYear(JObject ledgerFiscalYear)
        {
            var s = Stopwatch.StartNew();
            if (ledgerFiscalYear["id"] == null) ledgerFiscalYear["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting ledger fiscal year {ledgerFiscalYear["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledger-fiscal-years";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = ledgerFiscalYear.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public void UpdateLedgerFiscalYear(JObject ledgerFiscalYear)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating ledger fiscal year {ledgerFiscalYear["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledger-fiscal-years/{ledgerFiscalYear["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = ledgerFiscalYear.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteLedgerFiscalYear(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting ledger fiscal year {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/ledger-fiscal-years/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Libraries(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying libraries");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/libraries{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetLibrary(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting library {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/libraries/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertLibrary(JObject library)
        {
            var s = Stopwatch.StartNew();
            if (library["id"] == null) library["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting library {library["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/libraries";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = library.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteLibrary(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting library {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/libraries/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Loans(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying loans");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-storage/loans{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetLoan(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting loan {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-storage/loans/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertLoan(JObject loan)
        {
            var s = Stopwatch.StartNew();
            if (loan["id"] == null) loan["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting loan {loan["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-storage/loans";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = loan.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteLoan(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting loan {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-storage/loans/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> LoanPolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying loan policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-policy-storage/loan-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetLoanPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting loan policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-policy-storage/loan-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertLoanPolicy(JObject loanPolicy)
        {
            var s = Stopwatch.StartNew();
            if (loanPolicy["id"] == null) loanPolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting loan policy {loanPolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-policy-storage/loan-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = loanPolicy.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteLoanPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting loan policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-policy-storage/loan-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> LoanTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying loan types");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetLoanType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting loan type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertLoanType(JObject loanType)
        {
            var s = Stopwatch.StartNew();
            if (loanType["id"] == null) loanType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting loan type {loanType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = loanType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteLoanType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting loan type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Locations(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying locations");
            AuthenticateIfNecessary();
            var url = $"{Url}/locations{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetLocation(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting location {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/locations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertLocation(JObject location)
        {
            var s = Stopwatch.StartNew();
            if (location["id"] == null) location["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting location {location["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/locations";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = location.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteLocation(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting location {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/locations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Logins(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying logins");
            AuthenticateIfNecessary();
            var url = $"{Url}/authn/credentials{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}length={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetLogin(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting login {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/authn/credentials/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertLogin(JObject login)
        {
            var s = Stopwatch.StartNew();
            if (login["id"] == null) login["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting login {login["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/authn/credentials";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = login.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public void UpdateLogin(JObject login)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating login {login["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/authn/credentials/{login["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = login.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteLogin(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting login {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/authn/credentials/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> LostItemFeePolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying lost item fee policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/lost-item-fees-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetLostItemFeePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting lost item fee policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/lost-item-fees-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertLostItemFeePolicy(JObject lostItemFeePolicy)
        {
            var s = Stopwatch.StartNew();
            if (lostItemFeePolicy["id"] == null) lostItemFeePolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting lost item fee policy {lostItemFeePolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/lost-item-fees-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = lostItemFeePolicy.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteLostItemFeePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting lost item fee policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/lost-item-fees-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> MaterialTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying material types");
            AuthenticateIfNecessary();
            var url = $"{Url}/material-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetMaterialType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting material type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/material-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertMaterialType(JObject materialType)
        {
            var s = Stopwatch.StartNew();
            if (materialType["id"] == null) materialType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting material type {materialType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/material-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = materialType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteMaterialType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting material type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/material-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ModeOfIssuances(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying mode of issuances");
            AuthenticateIfNecessary();
            var url = $"{Url}/modes-of-issuance{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetModeOfIssuance(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting mode of issuance {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/modes-of-issuance/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertModeOfIssuance(JObject modeOfIssuance)
        {
            var s = Stopwatch.StartNew();
            if (modeOfIssuance["id"] == null) modeOfIssuance["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting mode of issuance {modeOfIssuance["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/modes-of-issuance";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = modeOfIssuance.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteModeOfIssuance(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting mode of issuance {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/modes-of-issuance/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> NatureOfContentTerms(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying nature of content terms");
            AuthenticateIfNecessary();
            var url = $"{Url}/nature-of-content-terms{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetNatureOfContentTerm(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting nature of content term {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/nature-of-content-terms/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertNatureOfContentTerm(JObject natureOfContentTerm)
        {
            var s = Stopwatch.StartNew();
            if (natureOfContentTerm["id"] == null) natureOfContentTerm["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting nature of content term {natureOfContentTerm["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/nature-of-content-terms";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = natureOfContentTerm.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteNatureOfContentTerm(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting nature of content term {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/nature-of-content-terms/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Orders(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying orders");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/purchase-orders{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetOrder(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting order {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/purchase-orders/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertOrder(JObject order)
        {
            var s = Stopwatch.StartNew();
            if (order["id"] == null) order["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting order {order["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/purchase-orders";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = order.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteOrder(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting order {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/purchase-orders/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> OrderInvoices(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying order invoices");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-invoice-relns{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetOrderInvoice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting order invoice {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-invoice-relns/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertOrderInvoice(JObject orderInvoice)
        {
            var s = Stopwatch.StartNew();
            if (orderInvoice["id"] == null) orderInvoice["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting order invoice {orderInvoice["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-invoice-relns";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = orderInvoice.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteOrderInvoice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting order invoice {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-invoice-relns/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> OrderItems(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying order items");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/po-lines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetOrderItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting order item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/po-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertOrderItem(JObject orderItem)
        {
            var s = Stopwatch.StartNew();
            if (orderItem["id"] == null) orderItem["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting order item {orderItem["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/po-lines";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = orderItem.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteOrderItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting order item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/po-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> OrderTemplates(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying order templates");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-templates{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetOrderTemplate(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting order template {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-templates/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertOrderTemplate(JObject orderTemplate)
        {
            var s = Stopwatch.StartNew();
            if (orderTemplate["id"] == null) orderTemplate["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting order template {orderTemplate["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-templates";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = orderTemplate.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteOrderTemplate(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting order template {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/order-templates/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Organizations(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying organizations");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/organizations{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetOrganization(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting organization {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/organizations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertOrganization(JObject organization)
        {
            var s = Stopwatch.StartNew();
            if (organization["id"] == null) organization["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting organization {organization["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/organizations";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = organization.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteOrganization(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting organization {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/organizations-storage/organizations/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> OverdueFinePolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying overdue fine policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/overdue-fines-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetOverdueFinePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting overdue fine policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/overdue-fines-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertOverdueFinePolicy(JObject overdueFinePolicy)
        {
            var s = Stopwatch.StartNew();
            if (overdueFinePolicy["id"] == null) overdueFinePolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting overdue fine policy {overdueFinePolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/overdue-fines-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = overdueFinePolicy.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteOverdueFinePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting overdue fine policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/overdue-fines-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Owners(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying owners");
            AuthenticateIfNecessary();
            var url = $"{Url}/owners{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetOwner(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting owner {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/owners/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertOwner(JObject owner)
        {
            var s = Stopwatch.StartNew();
            if (owner["id"] == null) owner["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting owner {owner["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/owners";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = owner.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteOwner(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting owner {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/owners/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> PatronActionSessions(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying patron action sessions");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-action-session-storage/patron-action-sessions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetPatronActionSession(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting patron action session {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-action-session-storage/patron-action-sessions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertPatronActionSession(JObject patronActionSession)
        {
            var s = Stopwatch.StartNew();
            if (patronActionSession["id"] == null) patronActionSession["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting patron action session {patronActionSession["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-action-session-storage/patron-action-sessions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = patronActionSession.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeletePatronActionSession(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting patron action session {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-action-session-storage/patron-action-sessions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> PatronBlockConditions(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying patron block conditions");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-conditions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetPatronBlockCondition(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting patron block condition {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-conditions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertPatronBlockCondition(JObject patronBlockCondition)
        {
            var s = Stopwatch.StartNew();
            if (patronBlockCondition[""] == null) patronBlockCondition[""] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting patron block condition {patronBlockCondition[""]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-conditions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = patronBlockCondition.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public void UpdatePatronBlockCondition(JObject patronBlockCondition)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating patron block condition {patronBlockCondition[""]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-conditions/{patronBlockCondition[""]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = patronBlockCondition.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeletePatronBlockCondition(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting patron block condition {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-conditions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> PatronBlockLimits(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying patron block limits");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-limits{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetPatronBlockLimit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting patron block limit {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-limits/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertPatronBlockLimit(JObject patronBlockLimit)
        {
            var s = Stopwatch.StartNew();
            if (patronBlockLimit[""] == null) patronBlockLimit[""] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting patron block limit {patronBlockLimit[""]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-limits";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = patronBlockLimit.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public void UpdatePatronBlockLimit(JObject patronBlockLimit)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating patron block limit {patronBlockLimit[""]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-limits/{patronBlockLimit[""]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = patronBlockLimit.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeletePatronBlockLimit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting patron block limit {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-block-limits/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> PatronNoticePolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying patron notice policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-notice-policy-storage/patron-notice-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetPatronNoticePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting patron notice policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-notice-policy-storage/patron-notice-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertPatronNoticePolicy(JObject patronNoticePolicy)
        {
            var s = Stopwatch.StartNew();
            if (patronNoticePolicy["id"] == null) patronNoticePolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting patron notice policy {patronNoticePolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-notice-policy-storage/patron-notice-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = patronNoticePolicy.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeletePatronNoticePolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting patron notice policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/patron-notice-policy-storage/patron-notice-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Payments(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying payments");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefineactions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetPayment(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting payment {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefineactions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertPayment(JObject payment)
        {
            var s = Stopwatch.StartNew();
            if (payment["id"] == null) payment["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting payment {payment["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefineactions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = payment.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeletePayment(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting payment {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/feefineactions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> PaymentMethods(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying payment methods");
            AuthenticateIfNecessary();
            var url = $"{Url}/payments{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetPaymentMethod(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting payment method {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/payments/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertPaymentMethod(JObject paymentMethod)
        {
            var s = Stopwatch.StartNew();
            if (paymentMethod["id"] == null) paymentMethod["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting payment method {paymentMethod["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/payments";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = paymentMethod.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeletePaymentMethod(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting payment method {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/payments/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Permissions(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying permissions");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/permissions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}length={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetPermission(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting permission {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/permissions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertPermission(JObject permission)
        {
            var s = Stopwatch.StartNew();
            if (permission["id"] == null) permission["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting permission {permission["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/permissions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = permission.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeletePermission(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting permission {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/permissions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> PermissionsUsers(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying permissions users");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}length={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetPermissionsUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting permissions user {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertPermissionsUser(JObject permissionsUser)
        {
            var s = Stopwatch.StartNew();
            if (permissionsUser["id"] == null) permissionsUser["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting permissions user {permissionsUser["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/users";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = permissionsUser.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeletePermissionsUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting permissions user {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Pieces(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying pieces");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/pieces{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetPiece(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting piece {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/pieces/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertPiece(JObject piece)
        {
            var s = Stopwatch.StartNew();
            if (piece["id"] == null) piece["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting piece {piece["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/pieces";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = piece.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeletePiece(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting piece {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/pieces/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> PrecedingSucceedingTitles(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying preceding succeeding titles");
            AuthenticateIfNecessary();
            var url = $"{Url}/preceding-succeeding-titles{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetPrecedingSucceedingTitle(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting preceding succeeding title {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/preceding-succeeding-titles/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertPrecedingSucceedingTitle(JObject precedingSucceedingTitle)
        {
            var s = Stopwatch.StartNew();
            if (precedingSucceedingTitle["id"] == null) precedingSucceedingTitle["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting preceding succeeding title {precedingSucceedingTitle["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/preceding-succeeding-titles";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = precedingSucceedingTitle.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeletePrecedingSucceedingTitle(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting preceding succeeding title {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/preceding-succeeding-titles/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Prefixes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying prefixes");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/prefixes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetPrefix(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting prefix {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/prefixes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertPrefix(JObject prefix)
        {
            var s = Stopwatch.StartNew();
            if (prefix["id"] == null) prefix["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting prefix {prefix["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/prefixes";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = prefix.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeletePrefix(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting prefix {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/prefixes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Proxies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying proxies");
            AuthenticateIfNecessary();
            var url = $"{Url}/proxiesfor{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetProxy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting proxy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/proxiesfor/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertProxy(JObject proxy)
        {
            var s = Stopwatch.StartNew();
            if (proxy["id"] == null) proxy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting proxy {proxy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/proxiesfor";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = proxy.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteProxy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting proxy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/proxiesfor/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ReasonsForClosures(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying reasons for closures");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/reasons-for-closure{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetReasonsForClosure(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting reasons for closure {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/reasons-for-closure/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertReasonsForClosure(JObject reasonsForClosure)
        {
            var s = Stopwatch.StartNew();
            if (reasonsForClosure["id"] == null) reasonsForClosure["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting reasons for closure {reasonsForClosure["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/reasons-for-closure";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = reasonsForClosure.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public void UpdateReasonsForClosure(JObject reasonsForClosure)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Updating reasons for closure {reasonsForClosure["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/reasons-for-closure/{reasonsForClosure["id"]}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = reasonsForClosure.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteReasonsForClosure(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting reasons for closure {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/reasons-for-closure/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Records(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying records");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/records{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetRecord(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting record {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/records/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertRecord(JObject record)
        {
            var s = Stopwatch.StartNew();
            if (record["id"] == null) record["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting record {record["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/records";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = record.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers?.ContentType?.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteRecord(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting record {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/records/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> RefundReasons(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying refund reasons");
            AuthenticateIfNecessary();
            var url = $"{Url}/refunds{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetRefundReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting refund reason {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/refunds/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertRefundReason(JObject refundReason)
        {
            var s = Stopwatch.StartNew();
            if (refundReason["id"] == null) refundReason["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting refund reason {refundReason["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/refunds";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = refundReason.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteRefundReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting refund reason {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/refunds/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ReportingCodes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying reporting codes");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/reporting-codes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetReportingCode(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting reporting code {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/reporting-codes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertReportingCode(JObject reportingCode)
        {
            var s = Stopwatch.StartNew();
            if (reportingCode["id"] == null) reportingCode["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting reporting code {reportingCode["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/reporting-codes";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = reportingCode.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteReportingCode(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting reporting code {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/reporting-codes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Requests(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying requests");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-storage/requests{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetRequest(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting request {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-storage/requests/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertRequest(JObject request)
        {
            var s = Stopwatch.StartNew();
            if (request["id"] == null) request["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting request {request["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-storage/requests";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = request.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteRequest(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting request {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-storage/requests/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> RequestPolicies(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying request policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-policy-storage/request-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetRequestPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting request policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-policy-storage/request-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertRequestPolicy(JObject requestPolicy)
        {
            var s = Stopwatch.StartNew();
            if (requestPolicy["id"] == null) requestPolicy["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting request policy {requestPolicy["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-policy-storage/request-policies";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = requestPolicy.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteRequestPolicy(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting request policy {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-policy-storage/request-policies/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ScheduledNotices(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying scheduled notices");
            AuthenticateIfNecessary();
            var url = $"{Url}/scheduled-notice-storage/scheduled-notices{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetScheduledNotice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting scheduled notice {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/scheduled-notice-storage/scheduled-notices/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertScheduledNotice(JObject scheduledNotice)
        {
            var s = Stopwatch.StartNew();
            if (scheduledNotice["id"] == null) scheduledNotice["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting scheduled notice {scheduledNotice["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/scheduled-notice-storage/scheduled-notices";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = scheduledNotice.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteScheduledNotice(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting scheduled notice {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/scheduled-notice-storage/scheduled-notices/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ServicePoints(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying service points");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetServicePoint(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting service point {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertServicePoint(JObject servicePoint)
        {
            var s = Stopwatch.StartNew();
            if (servicePoint["id"] == null) servicePoint["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting service point {servicePoint["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = servicePoint.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteServicePoint(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting service point {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> ServicePointUsers(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying service point users");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points-users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetServicePointUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting service point user {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points-users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertServicePointUser(JObject servicePointUser)
        {
            var s = Stopwatch.StartNew();
            if (servicePointUser["id"] == null) servicePointUser["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting service point user {servicePointUser["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points-users";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = servicePointUser.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteServicePointUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting service point user {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points-users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Snapshots(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying snapshots");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/snapshots{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetSnapshot(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting snapshot {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/snapshots/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertSnapshot(JObject snapshot)
        {
            var s = Stopwatch.StartNew();
            if (snapshot["jobExecutionId"] == null) snapshot["jobExecutionId"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting snapshot {snapshot["jobExecutionId"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/snapshots";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = snapshot.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteSnapshot(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting snapshot {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/source-storage/snapshots/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> StaffSlips(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying staff slips");
            AuthenticateIfNecessary();
            var url = $"{Url}/staff-slips-storage/staff-slips{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetStaffSlip(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting staff slip {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/staff-slips-storage/staff-slips/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertStaffSlip(JObject staffSlip)
        {
            var s = Stopwatch.StartNew();
            if (staffSlip["id"] == null) staffSlip["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting staff slip {staffSlip["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/staff-slips-storage/staff-slips";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = staffSlip.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteStaffSlip(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting staff slip {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/staff-slips-storage/staff-slips/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> StatisticalCodes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying statistical codes");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-codes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetStatisticalCode(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting statistical code {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-codes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertStatisticalCode(JObject statisticalCode)
        {
            var s = Stopwatch.StartNew();
            if (statisticalCode["id"] == null) statisticalCode["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting statistical code {statisticalCode["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-codes";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = statisticalCode.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteStatisticalCode(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting statistical code {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-codes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> StatisticalCodeTypes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying statistical code types");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-code-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetStatisticalCodeType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting statistical code type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-code-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertStatisticalCodeType(JObject statisticalCodeType)
        {
            var s = Stopwatch.StartNew();
            if (statisticalCodeType["id"] == null) statisticalCodeType["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting statistical code type {statisticalCodeType["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-code-types";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = statisticalCodeType.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteStatisticalCodeType(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting statistical code type {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-code-types/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Suffixes(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying suffixes");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/suffixes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetSuffix(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting suffix {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/suffixes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertSuffix(JObject suffix)
        {
            var s = Stopwatch.StartNew();
            if (suffix["id"] == null) suffix["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting suffix {suffix["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/suffixes";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = suffix.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteSuffix(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting suffix {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/configuration/suffixes/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Templates(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying templates");
            AuthenticateIfNecessary();
            var url = $"{Url}/templates{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetTemplate(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting template {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/templates/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertTemplate(JObject template)
        {
            var s = Stopwatch.StartNew();
            if (template["id"] == null) template["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting template {template["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/templates";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = template.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteTemplate(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting template {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/templates/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Titles(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying titles");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/titles{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetTitle(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting title {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/titles/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertTitle(JObject title)
        {
            var s = Stopwatch.StartNew();
            if (title["id"] == null) title["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting title {title["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/titles";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = title.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteTitle(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting title {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/orders-storage/titles/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Transactions(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying transactions");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/transactions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetTransaction(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting transaction {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/transactions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertTransaction(JObject transaction)
        {
            var s = Stopwatch.StartNew();
            if (transaction["id"] == null) transaction["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting transaction {transaction["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/transactions";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = transaction.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteTransaction(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting transaction {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/finance-storage/transactions/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> TransferAccounts(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying transfer accounts");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfers{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetTransferAccount(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting transfer account {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertTransferAccount(JObject transferAccount)
        {
            var s = Stopwatch.StartNew();
            if (transferAccount["id"] == null) transferAccount["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting transfer account {transferAccount["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfers";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = transferAccount.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteTransferAccount(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting transfer account {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> TransferCriterias(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying transfer criterias");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfer-criterias{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetTransferCriteria(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting transfer criteria {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfer-criterias/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertTransferCriteria(JObject transferCriteria)
        {
            var s = Stopwatch.StartNew();
            if (transferCriteria["id"] == null) transferCriteria["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting transfer criteria {transferCriteria["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfer-criterias";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = transferCriteria.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteTransferCriteria(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting transfer criteria {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/transfer-criterias/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Users(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying users");
            AuthenticateIfNecessary();
            var url = $"{Url}/users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read(); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting user {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertUser(JObject user)
        {
            var s = Stopwatch.StartNew();
            if (user["id"] == null) user["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting user {user["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/users";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = user.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject ImportUsers(IEnumerable<JObject> users, string source = null, bool disable = true, bool merge = true)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Importing users {string.Join(", ", users.Select(u => u["id"]))}");
            AuthenticateIfNecessary();
            var url = $"{Url}/user-import";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = new JObject(new JProperty("users", new JArray(users)), new JProperty("totalRecords", users.Count()), new JProperty("deactivateMissingUsers", disable), new JProperty("updateOnlyPresentFields", merge), new JProperty("sourceType", source)).ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers?.ContentType?.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public void DeleteUser(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting user {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/users/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> UserAcquisitionsUnits(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying user acquisitions units");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/memberships{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetUserAcquisitionsUnit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting user acquisitions unit {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/memberships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertUserAcquisitionsUnit(JObject userAcquisitionsUnit)
        {
            var s = Stopwatch.StartNew();
            if (userAcquisitionsUnit["id"] == null) userAcquisitionsUnit["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting user acquisitions unit {userAcquisitionsUnit["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/memberships";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = userAcquisitionsUnit.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteUserAcquisitionsUnit(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting user acquisitions unit {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/acquisitions-units-storage/memberships/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> UserRequestPreferences(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying user request preferences");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-preference-storage/request-preference{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetUserRequestPreference(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting user request preference {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-preference-storage/request-preference/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertUserRequestPreference(JObject userRequestPreference)
        {
            var s = Stopwatch.StartNew();
            if (userRequestPreference["id"] == null) userRequestPreference["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting user request preference {userRequestPreference["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-preference-storage/request-preference";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = userRequestPreference.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteUserRequestPreference(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting user request preference {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/request-preference-storage/request-preference/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> Vouchers(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying vouchers");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/vouchers{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetVoucher(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting voucher {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/vouchers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertVoucher(JObject voucher)
        {
            var s = Stopwatch.StartNew();
            if (voucher["id"] == null) voucher["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting voucher {voucher["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/vouchers";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = voucher.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteVoucher(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting voucher {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/vouchers/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> VoucherItems(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying voucher items");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/voucher-lines{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetVoucherItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting voucher item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/voucher-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertVoucherItem(JObject voucherItem)
        {
            var s = Stopwatch.StartNew();
            if (voucherItem["id"] == null) voucherItem["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting voucher item {voucherItem["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/voucher-lines";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = voucherItem.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteVoucherItem(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting voucher item {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/voucher-storage/voucher-lines/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public IEnumerable<JObject> WaiveReasons(string where = null, string orderBy = null, int? skip = null, int? take = null)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Querying waive reasons");
            AuthenticateIfNecessary();
            var url = $"{Url}/waives{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "cql.allrecords=1 ")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take ?? int.MaxValue}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
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
                if (!jtr.Read()) throw new InvalidDataException(hrm.Content.ReadAsStringAsync().Result); jtr.Read(); jtr.Read();
                var js = new JsonSerializer();
                while (jtr.Read() && jtr.TokenType != JsonToken.EndArray)
                {
                    var jo = (JObject)js.Deserialize(jtr);
                    traceSource.TraceEvent(TraceEventType.Verbose, 0, "{0}", jo);
                    yield return jo;
                }
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public JObject GetWaiveReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Getting waive reason {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/waives/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.GetAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.OK) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
            return jo;
        }

        public JObject InsertWaiveReason(JObject waiveReason)
        {
            var s = Stopwatch.StartNew();
            if (waiveReason["id"] == null) waiveReason["id"] = Guid.NewGuid();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Inserting waive reason {waiveReason["id"]}");
            AuthenticateIfNecessary();
            var url = $"{Url}/waives";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var s2 = waiveReason.ToString(formatting);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.Created) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void DeleteWaiveReason(string id)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Deleting waive reason {id}");
            AuthenticateIfNecessary();
            var url = $"{Url}/waives/{id}";
            traceSource.TraceEvent(TraceEventType.Verbose, 0, url);
            var hrm = httpClient.DeleteAsync(url).Result;
            var s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
        }

        public void Dispose()
        {
            if (httpClient != null) httpClient.Dispose();
        }
    }
}
