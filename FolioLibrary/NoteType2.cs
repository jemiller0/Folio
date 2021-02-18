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
    // uc.note_types -> diku_mod_notes.note_type
    // NoteType2 -> NoteType
    [DisplayColumn(nameof(Name)), DisplayName("Note Types"), JsonConverter(typeof(JsonPathJsonConverter<NoteType2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("note_types", Schema = "uc")]
    public partial class NoteType2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.NoteType.json")))
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

        [Column("usage_note_total"), Display(Name = "Usage Note Total", Order = 3), JsonProperty("usage.noteTotal"), Required]
        public virtual int? UsageNoteTotal { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 4), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 5), InverseProperty("NoteType2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 6), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 9), InverseProperty("NoteType2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 10), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(NoteType), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 12), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Notes", Order = 13)]
        public virtual ICollection<Note3> Note3s { get; set; }

        [Display(Name = "Notes 1", Order = 14)]
        public virtual ICollection<Note3> Note3s1 { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(UsageNoteTotal)} = {UsageNoteTotal}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static NoteType2 FromJObject(JObject jObject) => jObject != null ? new NoteType2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            UsageNoteTotal = (int?)jObject.SelectToken("usage.noteTotal"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("usage", new JObject(
                new JProperty("noteTotal", UsageNoteTotal))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}