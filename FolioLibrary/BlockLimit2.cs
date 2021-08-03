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
    // uc.block_limits -> diku_mod_patron_blocks.patron_block_limits
    // BlockLimit2 -> BlockLimit
    [DisplayColumn(nameof(Id)), DisplayName("Block Limits"), JsonConverter(typeof(JsonPathJsonConverter<BlockLimit2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("block_limits", Schema = "uc")]
    public partial class BlockLimit2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.BlockLimit.json")))
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
        public virtual Group2 Group { get; set; }

        [Column("group_id"), Display(Name = "Group", Order = 3), JsonProperty("patronGroupId"), Required]
        public virtual Guid? GroupId { get; set; }

        [Display(Order = 4)]
        public virtual BlockCondition2 Condition { get; set; }

        [Column("condition_id"), Display(Name = "Condition", Order = 5), JsonProperty("conditionId"), Required]
        public virtual Guid? ConditionId { get; set; }

        [Column("value"), Display(Order = 6), JsonProperty("value"), Required]
        public virtual decimal? Value { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 7), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 8), InverseProperty("BlockLimit2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 9), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 12), InverseProperty("BlockLimit2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 13), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(BlockLimit), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 15), Editable(false)]
        public virtual string Content { get; set; }

        [Column("conditionid"), ScaffoldColumn(false)]
        public virtual Guid? Conditionid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(GroupId)} = {GroupId}, {nameof(ConditionId)} = {ConditionId}, {nameof(Value)} = {Value}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(Conditionid)} = {Conditionid} }}";

        public static BlockLimit2 FromJObject(JObject jObject) => jObject != null ? new BlockLimit2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            GroupId = (Guid?)jObject.SelectToken("patronGroupId"),
            ConditionId = (Guid?)jObject.SelectToken("conditionId"),
            Value = (decimal?)jObject.SelectToken("value"),
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
            new JProperty("patronGroupId", GroupId),
            new JProperty("conditionId", ConditionId),
            new JProperty("value", Value),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
