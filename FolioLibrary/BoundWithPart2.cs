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
    // uc.bound_with_parts -> diku_mod_inventory_storage.bound_with_part
    // BoundWithPart2 -> BoundWithPart
    [DisplayColumn(nameof(Id)), DisplayName("Bound With Parts"), JsonConverter(typeof(JsonPathJsonConverter<BoundWithPart2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("bound_with_parts", Schema = "uc")]
    public partial class BoundWithPart2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.BoundWithPart.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id"), Display(Name = "Holding", Order = 3), JsonProperty("holdingsRecordId")]
        public virtual Guid? HoldingId { get; set; }

        [Display(Order = 4)]
        public virtual Item2 Item { get; set; }

        [Column("item_id"), Display(Name = "Item", Order = 5), JsonProperty("itemId")]
        public virtual Guid? ItemId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 6), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 7), InverseProperty("BoundWithPart2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 8), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 11), InverseProperty("BoundWithPart2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 12), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(BoundWithPart), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 14), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(HoldingId)} = {HoldingId}, {nameof(ItemId)} = {ItemId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static BoundWithPart2 FromJObject(JObject jObject) => jObject != null ? new BoundWithPart2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            HoldingId = (Guid?)jObject.SelectToken("holdingsRecordId"),
            ItemId = (Guid?)jObject.SelectToken("itemId"),
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
            new JProperty("holdingsRecordId", HoldingId),
            new JProperty("itemId", ItemId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
