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

namespace FolioLibrary
{
    public class FolioServiceClient : IDisposable
    {
        private string accessToken = ConfigurationManager.AppSettings["accessToken"];
        private HttpClient httpClient = new HttpClient();
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

        public IEnumerable<JObject> AddressTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding address types");
            AuthenticateIfNecessary();
            var url = $"{Url}/addresstypes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = addressType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = addressType.ToString();
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

        public IEnumerable<JObject> AlternativeTitleTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding alternative title types");
            AuthenticateIfNecessary();
            var url = $"{Url}/alternative-title-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = alternativeTitleType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = alternativeTitleType.ToString();
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

        public IEnumerable<JObject> CallNumberTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding call number types");
            AuthenticateIfNecessary();
            var url = $"{Url}/call-number-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = callNumberType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = callNumberType.ToString();
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

        public IEnumerable<JObject> Campuses(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding campuses");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/campuses{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = campus.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = campus.ToString();
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

        public IEnumerable<JObject> ClassificationTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding classification types");
            AuthenticateIfNecessary();
            var url = $"{Url}/classification-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = classificationType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = classificationType.ToString();
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

        public IEnumerable<JObject> ContributorNameTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding contributor name types");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-name-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = contributorNameType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = contributorNameType.ToString();
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

        public IEnumerable<JObject> ContributorTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding contributor types");
            AuthenticateIfNecessary();
            var url = $"{Url}/contributor-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = contributorType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = contributorType.ToString();
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

        public IEnumerable<JObject> ElectronicAccessRelationships(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding electronic access relationships");
            AuthenticateIfNecessary();
            var url = $"{Url}/electronic-access-relationships{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = electronicAccessRelationship.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = electronicAccessRelationship.ToString();
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

        public IEnumerable<JObject> Groups(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding groups");
            AuthenticateIfNecessary();
            var url = $"{Url}/groups{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = group.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = group.ToString();
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

        public IEnumerable<JObject> Holdings(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding holdings");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-storage/holdings{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = holding.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = holding.ToString();
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

        public IEnumerable<JObject> HoldingNoteTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding holding note types");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = holdingNoteType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = holdingNoteType.ToString();
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

        public IEnumerable<JObject> HoldingTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding holding types");
            AuthenticateIfNecessary();
            var url = $"{Url}/holdings-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = holdingType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = holdingType.ToString();
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

        public IEnumerable<JObject> IdTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding id types");
            AuthenticateIfNecessary();
            var url = $"{Url}/identifier-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = idType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = idType.ToString();
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

        public IEnumerable<JObject> IllPolicies(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding ill policies");
            AuthenticateIfNecessary();
            var url = $"{Url}/ill-policies{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = illPolicy.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = illPolicy.ToString();
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

        public IEnumerable<JObject> Instances(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding instances");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instances{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instance.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instance.ToString();
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

        public IEnumerable<JObject> InstanceFormats(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding instance formats");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-formats{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instanceFormat.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instanceFormat.ToString();
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

        public IEnumerable<JObject> InstanceRelationships(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding instance relationships");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-storage/instance-relationships{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instanceRelationship.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instanceRelationship.ToString();
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

        public IEnumerable<JObject> InstanceRelationshipTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding instance relationship types");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-relationship-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instanceRelationshipType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instanceRelationshipType.ToString();
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

        public IEnumerable<JObject> InstanceStatuses(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding instance statuses");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-statuses{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instanceStatus.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instanceStatus.ToString();
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

        public IEnumerable<JObject> InstanceTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding instance types");
            AuthenticateIfNecessary();
            var url = $"{Url}/instance-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instanceType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = instanceType.ToString();
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

        public IEnumerable<JObject> Institutions(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding institutions");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/institutions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = institution.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = institution.ToString();
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

        public IEnumerable<JObject> Items(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding items");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-storage/items{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = item.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = item.ToString();
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

        public IEnumerable<JObject> ItemNoteTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding item note types");
            AuthenticateIfNecessary();
            var url = $"{Url}/item-note-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = itemNoteType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = itemNoteType.ToString();
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

        public IEnumerable<JObject> Libraries(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding libraries");
            AuthenticateIfNecessary();
            var url = $"{Url}/location-units/libraries{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = library.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = library.ToString();
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

        public IEnumerable<JObject> LoanTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding loan types");
            AuthenticateIfNecessary();
            var url = $"{Url}/loan-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = loanType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = loanType.ToString();
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

        public IEnumerable<JObject> Locations(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding locations");
            AuthenticateIfNecessary();
            var url = $"{Url}/locations{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = location.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = location.ToString();
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

        public IEnumerable<JObject> Logins(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding logins");
            AuthenticateIfNecessary();
            var url = $"{Url}/authn/credentials{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = login.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = login.ToString();
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

        public IEnumerable<JObject> MaterialTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding material types");
            AuthenticateIfNecessary();
            var url = $"{Url}/material-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = materialType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = materialType.ToString();
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

        public IEnumerable<JObject> ModeOfIssuances(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding mode of issuances");
            AuthenticateIfNecessary();
            var url = $"{Url}/modes-of-issuance{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = modeOfIssuance.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = modeOfIssuance.ToString();
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

        public IEnumerable<JObject> Permissions(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding permissions");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/permissions{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = permission.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = permission.ToString();
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

        public IEnumerable<JObject> PermissionsUsers(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding permissions users");
            AuthenticateIfNecessary();
            var url = $"{Url}/perms/users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = permissionsUser.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = permissionsUser.ToString();
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

        public IEnumerable<JObject> Proxies(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding proxies");
            AuthenticateIfNecessary();
            var url = $"{Url}/proxiesfor{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = proxy.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = proxy.ToString();
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

        public IEnumerable<JObject> ServicePoints(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding service points");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = servicePoint.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = servicePoint.ToString();
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

        public IEnumerable<JObject> ServicePointUsers(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding service point users");
            AuthenticateIfNecessary();
            var url = $"{Url}/service-points-users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = servicePointUser.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = servicePointUser.ToString();
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

        public IEnumerable<JObject> StatisticalCodes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding statistical codes");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-codes{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = statisticalCode.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = statisticalCode.ToString();
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

        public IEnumerable<JObject> StatisticalCodeTypes(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding statistical code types");
            AuthenticateIfNecessary();
            var url = $"{Url}/statistical-code-types{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = statisticalCodeType.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = statisticalCodeType.ToString();
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

        public IEnumerable<JObject> Users(string where = null, string orderBy = null, int? skip = null, int? take = int.MaxValue)
        {
            var s = Stopwatch.StartNew();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, "Finding users");
            AuthenticateIfNecessary();
            var url = $"{Url}/users{(where != null || orderBy != null ? $"?query={WebUtility.UrlEncode(where)}{(orderBy != null ? $"{(where != null ? " " : "")}sortby {WebUtility.UrlEncode(orderBy)}" : "")}" : "")}{(skip != null ? $"{(where != null || orderBy != null ? "&" : "?")}offset={skip}" : "")}{(take != null ? $"{(where != null || orderBy != null || skip != null ? "&" : "?")}limit={take}" : "")}";
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
                jtr.Read(); jtr.Read(); jtr.Read();
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
            if (jo != null) s2 = jo.ToString();
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
            var s2 = user.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PostAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            var jo = hrm.Content.Headers.ContentType.MediaType == "application/json" ? JObject.Parse(s2) : null;
            if (jo != null) s2 = jo.ToString();
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
            var s2 = user.ToString();
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            var sc = new StringContent(s2, Encoding.UTF8, "application/json");
            var hrm = httpClient.PutAsync(url, sc).Result;
            s2 = hrm.Content.ReadAsStringAsync().Result;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s2);
            if (hrm.StatusCode != HttpStatusCode.NoContent) throw new HttpRequestException($"Response status code does not indicate success: {hrm.StatusCode} ({hrm.ReasonPhrase}).\r\n{s2}");
            traceSource.TraceEvent(TraceEventType.Verbose, 0, s.Elapsed.ToString());
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

        public void Dispose()
        {
            if (httpClient != null) httpClient.Dispose();
        }
    }
}
