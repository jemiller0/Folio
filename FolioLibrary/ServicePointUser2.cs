using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.service_point_users -> diku_mod_inventory_storage.service_point_user
    // ServicePointUser2 -> ServicePointUser
    [DisplayColumn(nameof(Id)), DisplayName("Service Point Users"), JsonConverter(typeof(JsonPathJsonConverter<ServicePointUser2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("service_point_users", Schema = "uc")]
    public partial class ServicePointUser2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.ServicePointUser.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2), InverseProperty("ServicePointUser2s2")]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3), JsonProperty("userId"), Required]
        public virtual Guid? UserId { get; set; }

        [Display(Name = "Default Service Point", Order = 4)]
        public virtual ServicePoint2 DefaultServicePoint { get; set; }

        [Column("default_service_point_id"), Display(Name = "Default Service Point", Order = 5), JsonProperty("defaultServicePointId")]
        public virtual Guid? DefaultServicePointId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 6), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 7), InverseProperty("ServicePointUser2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 8), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 11), InverseProperty("ServicePointUser2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 12), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(ServicePointUser), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 14), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Service Point User Service Points", Order = 15), JsonConverter(typeof(ArrayJsonConverter<List<ServicePointUserServicePoint>, ServicePointUserServicePoint>), "ServicePointId"), JsonProperty("servicePointsIds")]
        public virtual ICollection<ServicePointUserServicePoint> ServicePointUserServicePoints { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(DefaultServicePointId)} = {DefaultServicePointId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(ServicePointUserServicePoints)} = {(ServicePointUserServicePoints != null ? $"{{ {string.Join(", ", ServicePointUserServicePoints)} }}" : "")} }}";

        public static ServicePointUser2 FromJObject(JObject jObject) => jObject != null ? new ServicePointUser2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            UserId = (Guid?)jObject.SelectToken("userId"),
            DefaultServicePointId = (Guid?)jObject.SelectToken("defaultServicePointId"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            ServicePointUserServicePoints = jObject.SelectToken("servicePointsIds")?.Where(jt => jt.HasValues).Select(jt => ServicePointUserServicePoint.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("userId", UserId),
            new JProperty("defaultServicePointId", DefaultServicePointId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("servicePointsIds", ServicePointUserServicePoints?.Select(spusp => spusp.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
