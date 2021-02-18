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
    // uc.proxies -> diku_mod_users.proxyfor
    // Proxy2 -> Proxy
    [DisplayColumn(nameof(Id)), DisplayName("Proxies"), JsonConverter(typeof(JsonPathJsonConverter<Proxy2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("proxies", Schema = "uc")]
    public partial class Proxy2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Proxy.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2), InverseProperty("Proxy2s3")]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3), JsonProperty("userId")]
        public virtual Guid? UserId { get; set; }

        [Display(Name = "Proxy User", Order = 4), InverseProperty("Proxy2s1")]
        public virtual User2 ProxyUser { get; set; }

        [Column("proxy_user_id"), Display(Name = "Proxy User", Order = 5), JsonProperty("proxyUserId")]
        public virtual Guid? ProxyUserId { get; set; }

        [Column("request_for_sponsor"), Display(Name = "Request For Sponsor", Order = 6), JsonProperty("requestForSponsor"), StringLength(1024)]
        public virtual string RequestForSponsor { get; set; }

        [Column("notifications_to"), Display(Name = "Notifications To", Order = 7), JsonProperty("notificationsTo"), StringLength(1024)]
        public virtual string NotificationsTo { get; set; }

        [Column("accrue_to"), Display(Name = "Accrue To", Order = 8), JsonProperty("accrueTo"), StringLength(1024)]
        public virtual string AccrueTo { get; set; }

        [Column("status"), Display(Order = 9), JsonProperty("status"), StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("expiration_date"), DataType(DataType.Date), Display(Name = "Expiration Date", Order = 10), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("expirationDate")]
        public virtual DateTime? ExpirationDate { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 12), InverseProperty("Proxy2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 13), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 16), InverseProperty("Proxy2s2")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 17), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Proxy), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 19), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(ProxyUserId)} = {ProxyUserId}, {nameof(RequestForSponsor)} = {RequestForSponsor}, {nameof(NotificationsTo)} = {NotificationsTo}, {nameof(AccrueTo)} = {AccrueTo}, {nameof(Status)} = {Status}, {nameof(ExpirationDate)} = {ExpirationDate}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static Proxy2 FromJObject(JObject jObject) => jObject != null ? new Proxy2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            UserId = (Guid?)jObject.SelectToken("userId"),
            ProxyUserId = (Guid?)jObject.SelectToken("proxyUserId"),
            RequestForSponsor = (string)jObject.SelectToken("requestForSponsor"),
            NotificationsTo = (string)jObject.SelectToken("notificationsTo"),
            AccrueTo = (string)jObject.SelectToken("accrueTo"),
            Status = (string)jObject.SelectToken("status"),
            ExpirationDate = ((DateTime?)jObject.SelectToken("expirationDate"))?.ToLocalTime(),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("userId", UserId),
            new JProperty("proxyUserId", ProxyUserId),
            new JProperty("requestForSponsor", RequestForSponsor),
            new JProperty("notificationsTo", NotificationsTo),
            new JProperty("accrueTo", AccrueTo),
            new JProperty("status", Status),
            new JProperty("expirationDate", ExpirationDate?.ToUniversalTime()),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
