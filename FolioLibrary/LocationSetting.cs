using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FolioLibrary
{
    // uc.location_settings -> uc.configurations
    // LocationSetting -> Configuration2
    [CustomValidation(typeof(LocationSetting), nameof(ValidateLocationSetting)), DisplayColumn(nameof(Id)), DisplayName("Location Settings"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("location_settings", Schema = "uc")]
    public partial class LocationSetting
    {
        public static ValidationResult ValidateLocationSetting(LocationSetting locationSetting, ValidationContext context)
        {
            var fsc = (FolioServiceContext)context.ObjectInstance;
            if (locationSetting.LocationId != null && fsc.LocationSettings($"id <> \"{locationSetting.Id}\"").Any(ls => ls.LocationId == locationSetting.LocationId)) return new ValidationResult("Location already exists");
            return ValidationResult.Success;
        }

        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Location2 Location { get; set; }

        [Column("location_id"), Display(Name = "Location", Order = 3), Required]
        public virtual Guid? LocationId { get; set; }

        [Display(Order = 4)]
        public virtual Setting Settings { get; set; }

        [Column("settings_id"), Display(Name = "Settings", Order = 5), Required]
        public virtual Guid? SettingsId { get; set; }

        [Column("enabled"), Display(Order = 6)]
        public virtual bool? Enabled { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 7), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 8), InverseProperty("LocationSettings")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 9), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 11), InverseProperty("LocationSettings1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 12), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(LocationId)} = {LocationId}, {nameof(SettingsId)} = {SettingsId}, {nameof(Enabled)} = {Enabled}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId} }}";

        public static LocationSetting FromJObject(JObject jObject)
        {
            if (jObject == null) return null;
            var jo = JsonConvert.DeserializeObject<JObject>((string)jObject["value"]);
            return new LocationSetting
            {
                Id = (Guid?)jObject.SelectToken("id"),
                Enabled = (bool?)jObject.SelectToken("enabled"),
                LocationId = (Guid?)jo.SelectToken("locationId"),
                SettingsId = (Guid?)jo.SelectToken("settingsId"),
                CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
                CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
                LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
                LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId")
            };
        }

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("module", "uc"),
            new JProperty("configName", "location_settings"),
            new JProperty("code", Id),
            new JProperty("enabled", Enabled),
            new JProperty("value", new JObject(
                new JProperty("locationId", LocationId),
                new JProperty("settingsId", SettingsId)).ToString()),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId)))).RemoveNullAndEmptyProperties();
    }
}
