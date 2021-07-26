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
    // uc.user_request_preferences -> diku_mod_circulation_storage.user_request_preference
    // UserRequestPreference2 -> UserRequestPreference
    [DisplayColumn(nameof(Id)), DisplayName("User Request Preferences"), JsonConverter(typeof(JsonPathJsonConverter<UserRequestPreference2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("user_request_preferences", Schema = "uc")]
    public partial class UserRequestPreference2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.UserRequestPreference.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2), InverseProperty("UserRequestPreference2s2")]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3), JsonProperty("userId"), Required]
        public virtual Guid? UserId { get; set; }

        [Column("hold_shelf"), Display(Name = "Hold Shelf", Order = 4), JsonProperty("holdShelf")]
        public virtual bool? HoldShelf { get; set; }

        [Column("delivery"), Display(Order = 5), JsonProperty("delivery")]
        public virtual bool? Delivery { get; set; }

        [Display(Name = "Default Service Point", Order = 6)]
        public virtual ServicePoint2 DefaultServicePoint { get; set; }

        [Column("default_service_point_id"), Display(Name = "Default Service Point", Order = 7), JsonProperty("defaultServicePointId")]
        public virtual Guid? DefaultServicePointId { get; set; }

        [Display(Name = "Default Delivery Address Type", Order = 8)]
        public virtual AddressType2 DefaultDeliveryAddressType { get; set; }

        [Column("default_delivery_address_type_id"), Display(Name = "Default Delivery Address Type", Order = 9), JsonProperty("defaultDeliveryAddressTypeId")]
        public virtual Guid? DefaultDeliveryAddressTypeId { get; set; }

        [Column("fulfillment"), Display(Order = 10), JsonProperty("fulfillment"), RegularExpression(@"^(Delivery|Hold Shelf)$"), StringLength(1024)]
        public virtual string Fulfillment { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 12), InverseProperty("UserRequestPreference2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 13), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 16), InverseProperty("UserRequestPreference2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 17), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(UserRequestPreference), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 19), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(HoldShelf)} = {HoldShelf}, {nameof(Delivery)} = {Delivery}, {nameof(DefaultServicePointId)} = {DefaultServicePointId}, {nameof(DefaultDeliveryAddressTypeId)} = {DefaultDeliveryAddressTypeId}, {nameof(Fulfillment)} = {Fulfillment}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static UserRequestPreference2 FromJObject(JObject jObject) => jObject != null ? new UserRequestPreference2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            UserId = (Guid?)jObject.SelectToken("userId"),
            HoldShelf = (bool?)jObject.SelectToken("holdShelf"),
            Delivery = (bool?)jObject.SelectToken("delivery"),
            DefaultServicePointId = (Guid?)jObject.SelectToken("defaultServicePointId"),
            DefaultDeliveryAddressTypeId = (Guid?)jObject.SelectToken("defaultDeliveryAddressTypeId"),
            Fulfillment = (string)jObject.SelectToken("fulfillment"),
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
            new JProperty("userId", UserId),
            new JProperty("holdShelf", HoldShelf),
            new JProperty("delivery", Delivery),
            new JProperty("defaultServicePointId", DefaultServicePointId),
            new JProperty("defaultDeliveryAddressTypeId", DefaultDeliveryAddressTypeId),
            new JProperty("fulfillment", Fulfillment),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
