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
    // uc.hrid_settings -> diku_mod_inventory_storage.hrid_settings
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

        [Column("holdings_prefix"), Display(Name = "Holdings Prefix", Order = 4), JsonProperty("holdings.prefix"), StringLength(1024)]
        public virtual string HoldingsPrefix { get; set; }

        [Column("holdings_start_number"), Display(Name = "Holdings Start Number", Order = 5), JsonProperty("holdings.startNumber"), Required]
        public virtual int? HoldingsStartNumber { get; set; }

        [Column("items_prefix"), Display(Name = "Items Prefix", Order = 6), JsonProperty("items.prefix"), StringLength(1024)]
        public virtual string ItemsPrefix { get; set; }

        [Column("items_start_number"), Display(Name = "Items Start Number", Order = 7), JsonProperty("items.startNumber"), Required]
        public virtual int? ItemsStartNumber { get; set; }

        [Column("common_retain_leading_zeroes"), Display(Name = "Zero Pad", Order = 8), JsonProperty("commonRetainLeadingZeroes")]
        public virtual bool? ZeroPad { get; set; }

        [Column("content"), CustomValidation(typeof(HridSetting), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 9), Editable(false)]
        public virtual string Content { get; set; }

        [Column("lock"), Display(Order = 10)]
        public virtual bool? Lock { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstancesPrefix)} = {InstancesPrefix}, {nameof(InstancesStartNumber)} = {InstancesStartNumber}, {nameof(HoldingsPrefix)} = {HoldingsPrefix}, {nameof(HoldingsStartNumber)} = {HoldingsStartNumber}, {nameof(ItemsPrefix)} = {ItemsPrefix}, {nameof(ItemsStartNumber)} = {ItemsStartNumber}, {nameof(ZeroPad)} = {ZeroPad}, {nameof(Content)} = {Content}, {nameof(Lock)} = {Lock} }}";

        public static HridSetting2 FromJObject(JObject jObject) => jObject != null ? new HridSetting2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            InstancesPrefix = (string)jObject.SelectToken("instances.prefix"),
            InstancesStartNumber = (int?)jObject.SelectToken("instances.startNumber"),
            HoldingsPrefix = (string)jObject.SelectToken("holdings.prefix"),
            HoldingsStartNumber = (int?)jObject.SelectToken("holdings.startNumber"),
            ItemsPrefix = (string)jObject.SelectToken("items.prefix"),
            ItemsStartNumber = (int?)jObject.SelectToken("items.startNumber"),
            ZeroPad = (bool?)jObject.SelectToken("commonRetainLeadingZeroes"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("instances", new JObject(
                new JProperty("prefix", InstancesPrefix),
                new JProperty("startNumber", InstancesStartNumber))),
            new JProperty("holdings", new JObject(
                new JProperty("prefix", HoldingsPrefix),
                new JProperty("startNumber", HoldingsStartNumber))),
            new JProperty("items", new JObject(
                new JProperty("prefix", ItemsPrefix),
                new JProperty("startNumber", ItemsStartNumber))),
            new JProperty("commonRetainLeadingZeroes", ZeroPad)).RemoveNullAndEmptyProperties();
    }
}
