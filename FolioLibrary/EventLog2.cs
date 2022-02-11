using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.event_logs -> uchicago_mod_login.event_logs
    // EventLog2 -> EventLog
    [DisplayColumn(nameof(Id)), DisplayName("Event Logs"), JsonConverter(typeof(JsonPathJsonConverter<EventLog2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("event_logs", Schema = "uc")]
    public partial class EventLog2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.EventLog.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("tenant"), Display(Order = 2), JsonProperty("tenant"), Required, StringLength(1024)]
        public virtual string Tenant { get; set; }

        [Display(Order = 3), InverseProperty("EventLog2s2")]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 4), JsonProperty("userId"), Required]
        public virtual Guid? UserId { get; set; }

        [Column("ip"), Display(Order = 5), JsonProperty("ip"), StringLength(1024)]
        public virtual string Ip { get; set; }

        [Column("browser_information"), Display(Name = "Browser Information", Order = 6), JsonProperty("browserInformation"), StringLength(1024)]
        public virtual string BrowserInformation { get; set; }

        [Column("timestamp"), DataType(DataType.DateTime), Display(Order = 7), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("timestamp")]
        public virtual DateTime? Timestamp { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 9), InverseProperty("EventLog2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 10), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 13), InverseProperty("EventLog2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 14), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(EventLog), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 16), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Tenant)} = {Tenant}, {nameof(UserId)} = {UserId}, {nameof(Ip)} = {Ip}, {nameof(BrowserInformation)} = {BrowserInformation}, {nameof(Timestamp)} = {Timestamp}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static EventLog2 FromJObject(JObject jObject) => jObject != null ? new EventLog2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Tenant = (string)jObject.SelectToken("tenant"),
            UserId = (Guid?)jObject.SelectToken("userId"),
            Ip = (string)jObject.SelectToken("ip"),
            BrowserInformation = (string)jObject.SelectToken("browserInformation"),
            Timestamp = (DateTime?)jObject.SelectToken("timestamp"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("tenant", Tenant),
            new JProperty("userId", UserId),
            new JProperty("ip", Ip),
            new JProperty("browserInformation", BrowserInformation),
            new JProperty("timestamp", Timestamp?.ToLocalTime()),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
