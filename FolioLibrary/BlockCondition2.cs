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
    // uc.block_conditions -> diku_mod_patron_blocks.patron_block_conditions
    // BlockCondition2 -> BlockCondition
    [DisplayColumn(nameof(Name)), DisplayName("Block Conditions"), JsonConverter(typeof(JsonPathJsonConverter<BlockCondition2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("block_conditions", Schema = "uc")]
    public partial class BlockCondition2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.BlockCondition.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("block_borrowing"), Display(Name = "Block Borrowing", Order = 3), JsonProperty("blockBorrowing")]
        public virtual bool? BlockBorrowing { get; set; }

        [Column("block_renewals"), Display(Name = "Block Renewals", Order = 4), JsonProperty("blockRenewals")]
        public virtual bool? BlockRenewals { get; set; }

        [Column("block_requests"), Display(Name = "Block Requests", Order = 5), JsonProperty("blockRequests")]
        public virtual bool? BlockRequests { get; set; }

        [Column("value_type"), Display(Name = "Value Type", Order = 6), JsonProperty("valueType"), RegularExpression(@"^(Integer|Double)$"), Required, StringLength(1024)]
        public virtual string ValueType { get; set; }

        [Column("message"), Display(Order = 7), JsonProperty("message"), StringLength(1024)]
        public virtual string Message { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 9), InverseProperty("BlockCondition2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 10), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 13), InverseProperty("BlockCondition2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 14), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(BlockCondition), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 16), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Block Limits", Order = 17)]
        public virtual ICollection<BlockLimit2> BlockLimit2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(BlockBorrowing)} = {BlockBorrowing}, {nameof(BlockRenewals)} = {BlockRenewals}, {nameof(BlockRequests)} = {BlockRequests}, {nameof(ValueType)} = {ValueType}, {nameof(Message)} = {Message}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static BlockCondition2 FromJObject(JObject jObject) => jObject != null ? new BlockCondition2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            BlockBorrowing = (bool?)jObject.SelectToken("blockBorrowing"),
            BlockRenewals = (bool?)jObject.SelectToken("blockRenewals"),
            BlockRequests = (bool?)jObject.SelectToken("blockRequests"),
            ValueType = (string)jObject.SelectToken("valueType"),
            Message = (string)jObject.SelectToken("message"),
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
            new JProperty("name", Name),
            new JProperty("blockBorrowing", BlockBorrowing),
            new JProperty("blockRenewals", BlockRenewals),
            new JProperty("blockRequests", BlockRequests),
            new JProperty("valueType", ValueType),
            new JProperty("message", Message),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
