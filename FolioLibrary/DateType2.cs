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
    // uc.date_types -> uchicago_mod_inventory_storage.instance_date_type
    // DateType2 -> DateType
    [DisplayColumn(nameof(Name)), DisplayName("Date Types"), JsonConverter(typeof(JsonPathJsonConverter<DateType2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("date_types", Schema = "uc")]
    public partial class DateType2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.DateType.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), Editable(false), JsonProperty("name"), StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("code"), Display(Order = 3), Editable(false), JsonProperty("code"), StringLength(1)]
        public virtual string Code { get; set; }

        [Column("display_format_delimiter"), Display(Name = "Display Format Delimiter", Order = 4), Editable(false), JsonProperty("displayFormat.delimiter"), StringLength(1024)]
        public virtual string DisplayFormatDelimiter { get; set; }

        [Column("display_format_keep_delimiter"), Display(Name = "Display Format Keep Delimiter", Order = 5), Editable(false), JsonProperty("displayFormat.keepDelimiter")]
        public virtual bool? DisplayFormatKeepDelimiter { get; set; }

        [Column("source"), Display(Order = 6), Editable(false), JsonProperty("source"), RegularExpression(@"^(folio|local)$"), StringLength(1024)]
        public virtual string Source { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 7), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 8), InverseProperty("DateType2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 9), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 12), InverseProperty("DateType2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 13), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(DateType), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 15), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Instances", Order = 16)]
        public virtual ICollection<Instance2> Instance2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(DisplayFormatDelimiter)} = {DisplayFormatDelimiter}, {nameof(DisplayFormatKeepDelimiter)} = {DisplayFormatKeepDelimiter}, {nameof(Source)} = {Source}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static DateType2 FromJObject(JObject jObject) => jObject != null ? new DateType2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Code = (string)jObject.SelectToken("code"),
            DisplayFormatDelimiter = (string)jObject.SelectToken("displayFormat.delimiter"),
            DisplayFormatKeepDelimiter = (bool?)jObject.SelectToken("displayFormat.keepDelimiter"),
            Source = (string)jObject.SelectToken("source"),
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
            new JProperty("code", Code),
            new JProperty("displayFormat", new JObject(
                new JProperty("delimiter", DisplayFormatDelimiter),
                new JProperty("keepDelimiter", DisplayFormatKeepDelimiter))),
            new JProperty("source", Source),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
