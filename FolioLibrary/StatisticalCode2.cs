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
    // uc.statistical_codes -> uchicago_mod_inventory_storage.statistical_code
    // StatisticalCode2 -> StatisticalCode
    [DisplayColumn(nameof(Name)), DisplayName("Statistical Codes"), JsonConverter(typeof(JsonPathJsonConverter<StatisticalCode2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("statistical_codes", Schema = "uc")]
    public partial class StatisticalCode2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.StatisticalCode.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("code"), Display(Order = 2), JsonProperty("code"), Required, StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("name"), Display(Order = 3), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Display(Name = "Statistical Code Type", Order = 4)]
        public virtual StatisticalCodeType2 StatisticalCodeType { get; set; }

        [Column("statistical_code_type_id"), Display(Name = "Statistical Code Type", Order = 5), JsonProperty("statisticalCodeTypeId"), Required]
        public virtual Guid? StatisticalCodeTypeId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 6), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 7), InverseProperty("StatisticalCode2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 8), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 11), InverseProperty("StatisticalCode2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 12), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(StatisticalCode), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 14), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Holding Statistical Codes", Order = 15)]
        public virtual ICollection<HoldingStatisticalCode> HoldingStatisticalCodes { get; set; }

        [Display(Name = "Instance Statistical Codes", Order = 16)]
        public virtual ICollection<InstanceStatisticalCode> InstanceStatisticalCodes { get; set; }

        [Display(Name = "Item Statistical Codes", Order = 17)]
        public virtual ICollection<ItemStatisticalCode> ItemStatisticalCodes { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Code)} = {Code}, {nameof(Name)} = {Name}, {nameof(StatisticalCodeTypeId)} = {StatisticalCodeTypeId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static StatisticalCode2 FromJObject(JObject jObject) => jObject != null ? new StatisticalCode2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Code = (string)jObject.SelectToken("code"),
            Name = (string)jObject.SelectToken("name"),
            StatisticalCodeTypeId = (Guid?)jObject.SelectToken("statisticalCodeTypeId"),
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
            new JProperty("code", Code),
            new JProperty("name", Name),
            new JProperty("statisticalCodeTypeId", StatisticalCodeTypeId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
