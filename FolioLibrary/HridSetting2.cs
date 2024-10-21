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
    // uc.hrid_settings -> uchicago_mod_inventory_storage.hrid_settings
    // HridSetting2 -> HridSetting
    [DisplayColumn(nameof(Id)), DisplayName("HRID Settings"), JsonConverter(typeof(JsonPathJsonConverter<HridSetting2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("hrid_settings", Schema = "uc")]
    public partial class HridSetting2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.HridSetting.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("instances_prefix"), Display(Name = "Instances Prefix", Order = 2), JsonProperty("instances.prefix"), StringLength(1024)]
        public virtual string InstancesPrefix { get; set; }

        [Column("instances_start_number"), Display(Name = "Instances Start Number", Order = 3), JsonProperty("instances.startNumber"), Required]
        public virtual int? InstancesStartNumber { get; set; }

        [Column("instances_current_number"), Display(Name = "Instances Current Number", Order = 4), Editable(false), JsonProperty("instances.currentNumber")]
        public virtual int? InstancesCurrentNumber { get; set; }

        [Column("holdings_prefix"), Display(Name = "Holdings Prefix", Order = 5), JsonProperty("holdings.prefix"), StringLength(1024)]
        public virtual string HoldingsPrefix { get; set; }

        [Column("holdings_start_number"), Display(Name = "Holdings Start Number", Order = 6), JsonProperty("holdings.startNumber"), Required]
        public virtual int? HoldingsStartNumber { get; set; }

        [Column("holdings_current_number"), Display(Name = "Holdings Current Number", Order = 7), Editable(false), JsonProperty("holdings.currentNumber")]
        public virtual int? HoldingsCurrentNumber { get; set; }

        [Column("items_prefix"), Display(Name = "Items Prefix", Order = 8), JsonProperty("items.prefix"), StringLength(1024)]
        public virtual string ItemsPrefix { get; set; }

        [Column("items_start_number"), Display(Name = "Items Start Number", Order = 9), JsonProperty("items.startNumber"), Required]
        public virtual int? ItemsStartNumber { get; set; }

        [Column("items_current_number"), Display(Name = "Items Current Number", Order = 10), Editable(false), JsonProperty("items.currentNumber")]
        public virtual int? ItemsCurrentNumber { get; set; }

        [Column("common_retain_leading_zeroes"), Display(Name = "Zero Pad", Order = 11), JsonProperty("commonRetainLeadingZeroes")]
        public virtual bool? ZeroPad { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 13), InverseProperty("HridSetting2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 14), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 16), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 17), InverseProperty("HridSetting2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 18), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(HridSetting), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 20), Editable(false)]
        public virtual string Content { get; set; }

        [Column("lock"), Display(Order = 21)]
        public virtual bool? Lock { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstancesPrefix)} = {InstancesPrefix}, {nameof(InstancesStartNumber)} = {InstancesStartNumber}, {nameof(InstancesCurrentNumber)} = {InstancesCurrentNumber}, {nameof(HoldingsPrefix)} = {HoldingsPrefix}, {nameof(HoldingsStartNumber)} = {HoldingsStartNumber}, {nameof(HoldingsCurrentNumber)} = {HoldingsCurrentNumber}, {nameof(ItemsPrefix)} = {ItemsPrefix}, {nameof(ItemsStartNumber)} = {ItemsStartNumber}, {nameof(ItemsCurrentNumber)} = {ItemsCurrentNumber}, {nameof(ZeroPad)} = {ZeroPad}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(Lock)} = {Lock} }}";

        public static HridSetting2 FromJObject(JObject jObject) => jObject != null ? new HridSetting2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            InstancesPrefix = (string)jObject.SelectToken("instances.prefix"),
            InstancesStartNumber = (int?)jObject.SelectToken("instances.startNumber"),
            InstancesCurrentNumber = (int?)jObject.SelectToken("instances.currentNumber"),
            HoldingsPrefix = (string)jObject.SelectToken("holdings.prefix"),
            HoldingsStartNumber = (int?)jObject.SelectToken("holdings.startNumber"),
            HoldingsCurrentNumber = (int?)jObject.SelectToken("holdings.currentNumber"),
            ItemsPrefix = (string)jObject.SelectToken("items.prefix"),
            ItemsStartNumber = (int?)jObject.SelectToken("items.startNumber"),
            ItemsCurrentNumber = (int?)jObject.SelectToken("items.currentNumber"),
            ZeroPad = (bool?)jObject.SelectToken("commonRetainLeadingZeroes"),
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
            new JProperty("instances", new JObject(
                new JProperty("prefix", InstancesPrefix),
                new JProperty("startNumber", InstancesStartNumber),
                new JProperty("currentNumber", InstancesCurrentNumber))),
            new JProperty("holdings", new JObject(
                new JProperty("prefix", HoldingsPrefix),
                new JProperty("startNumber", HoldingsStartNumber),
                new JProperty("currentNumber", HoldingsCurrentNumber))),
            new JProperty("items", new JObject(
                new JProperty("prefix", ItemsPrefix),
                new JProperty("startNumber", ItemsStartNumber),
                new JProperty("currentNumber", ItemsCurrentNumber))),
            new JProperty("commonRetainLeadingZeroes", ZeroPad),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
