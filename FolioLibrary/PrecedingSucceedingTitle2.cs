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
    // uc.preceding_succeeding_titles -> uchicago_mod_inventory_storage.preceding_succeeding_title
    // PrecedingSucceedingTitle2 -> PrecedingSucceedingTitle
    [DisplayColumn(nameof(Title)), DisplayName("Preceding Succeeding Titles"), JsonConverter(typeof(JsonPathJsonConverter<PrecedingSucceedingTitle2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("preceding_succeeding_titles", Schema = "uc")]
    public partial class PrecedingSucceedingTitle2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.PrecedingSucceedingTitle.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Preceding Instance", Order = 2), InverseProperty("PrecedingSucceedingTitle2s")]
        public virtual Instance2 PrecedingInstance { get; set; }

        [Column("preceding_instance_id"), Display(Name = "Preceding Instance", Order = 3), JsonProperty("precedingInstanceId")]
        public virtual Guid? PrecedingInstanceId { get; set; }

        [Display(Name = "Succeeding Instance", Order = 4), InverseProperty("PrecedingSucceedingTitle2s1")]
        public virtual Instance2 SucceedingInstance { get; set; }

        [Column("succeeding_instance_id"), Display(Name = "Succeeding Instance", Order = 5), JsonProperty("succeedingInstanceId")]
        public virtual Guid? SucceedingInstanceId { get; set; }

        [Column("title"), Display(Order = 6), JsonProperty("title"), StringLength(1024)]
        public virtual string Title { get; set; }

        [Column("hrid"), Display(Order = 7), JsonProperty("hrid"), StringLength(1024)]
        public virtual string Hrid { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 9), InverseProperty("PrecedingSucceedingTitle2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 10), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 13), InverseProperty("PrecedingSucceedingTitle2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 14), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(PrecedingSucceedingTitle), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 16), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Preceding Succeeding Title Identifiers", Order = 17), JsonProperty("identifiers")]
        public virtual ICollection<PrecedingSucceedingTitleIdentifier> PrecedingSucceedingTitleIdentifiers { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(PrecedingInstanceId)} = {PrecedingInstanceId}, {nameof(SucceedingInstanceId)} = {SucceedingInstanceId}, {nameof(Title)} = {Title}, {nameof(Hrid)} = {Hrid}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(PrecedingSucceedingTitleIdentifiers)} = {(PrecedingSucceedingTitleIdentifiers != null ? $"{{ {string.Join(", ", PrecedingSucceedingTitleIdentifiers)} }}" : "")} }}";

        public static PrecedingSucceedingTitle2 FromJObject(JObject jObject) => jObject != null ? new PrecedingSucceedingTitle2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            PrecedingInstanceId = (Guid?)jObject.SelectToken("precedingInstanceId"),
            SucceedingInstanceId = (Guid?)jObject.SelectToken("succeedingInstanceId"),
            Title = (string)jObject.SelectToken("title"),
            Hrid = (string)jObject.SelectToken("hrid"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            PrecedingSucceedingTitleIdentifiers = jObject.SelectToken("identifiers")?.Where(jt => jt.HasValues).Select(jt => PrecedingSucceedingTitleIdentifier.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("precedingInstanceId", PrecedingInstanceId),
            new JProperty("succeedingInstanceId", SucceedingInstanceId),
            new JProperty("title", Title),
            new JProperty("hrid", Hrid),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("identifiers", PrecedingSucceedingTitleIdentifiers?.Select(psti => psti.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
