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
    // uc.notes2 -> diku_mod_notes.note_data
    // Note3 -> Note
    [DisplayColumn(nameof(Title)), DisplayName("Notes"), JsonConverter(typeof(JsonPathJsonConverter<Note3>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("notes2", Schema = "uc")]
    public partial class Note3
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Note.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Type 1", Order = 2), InverseProperty("Note3s1")]
        public virtual NoteType2 Type1 { get; set; }

        [Column("type_id"), Display(Name = "Type 1", Order = 3), ForeignKey("Type1"), JsonProperty("typeId"), Required]
        public virtual Guid? TypeId { get; set; }

        [Column("type"), Display(Order = 4), Editable(false), JsonProperty("type"), StringLength(1024)]
        public virtual string Type { get; set; }

        [Column("domain"), Display(Order = 5), JsonProperty("domain"), Required, StringLength(1024)]
        public virtual string Domain { get; set; }

        [Column("title"), Display(Order = 6), JsonProperty("title"), Required, StringLength(255)]
        public virtual string Title { get; set; }

        [Column("content2"), Display(Name = "Content 2", Order = 7), JsonProperty("content"), StringLength(1024)]
        public virtual string Content2 { get; set; }

        [Column("status"), Display(Order = 8), Editable(false), JsonProperty("status"), RegularExpression(@"^(ASSIGNED|UNASSIGNED)$"), StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("creator_last_name"), Display(Name = "Creator Last Name", Order = 9), Editable(false), JsonProperty("creator.lastName"), StringLength(1024)]
        public virtual string CreatorLastName { get; set; }

        [Column("creator_first_name"), Display(Name = "Creator First Name", Order = 10), Editable(false), JsonProperty("creator.firstName"), StringLength(1024)]
        public virtual string CreatorFirstName { get; set; }

        [Column("creator_middle_name"), Display(Name = "Creator Middle Name", Order = 11), Editable(false), JsonProperty("creator.middleName"), StringLength(1024)]
        public virtual string CreatorMiddleName { get; set; }

        [Column("updater_last_name"), Display(Name = "Updater Last Name", Order = 12), Editable(false), JsonProperty("updater.lastName"), StringLength(1024)]
        public virtual string UpdaterLastName { get; set; }

        [Column("updater_first_name"), Display(Name = "Updater First Name", Order = 13), Editable(false), JsonProperty("updater.firstName"), StringLength(1024)]
        public virtual string UpdaterFirstName { get; set; }

        [Column("updater_middle_name"), Display(Name = "Updater Middle Name", Order = 14), Editable(false), JsonProperty("updater.middleName"), StringLength(1024)]
        public virtual string UpdaterMiddleName { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 16), InverseProperty("Note3s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 17), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 19), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 20), InverseProperty("Note3s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 21), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Note), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 23), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Temporary Type", Order = 24), InverseProperty("Note3s")]
        public virtual NoteType2 TemporaryType { get; set; }

        [Column("temporary_type_id"), Display(Name = "Temporary Type", Order = 25)]
        public virtual Guid? TemporaryTypeId { get; set; }

        [Column("search_content"), Display(Name = "Search Content", Order = 26), StringLength(1024)]
        public virtual string SearchContent { get; set; }

        [Display(Name = "Note Links", Order = 27), JsonProperty("links")]
        public virtual ICollection<NoteLink> NoteLinks { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(TypeId)} = {TypeId}, {nameof(Type)} = {Type}, {nameof(Domain)} = {Domain}, {nameof(Title)} = {Title}, {nameof(Content2)} = {Content2}, {nameof(Status)} = {Status}, {nameof(CreatorLastName)} = {CreatorLastName}, {nameof(CreatorFirstName)} = {CreatorFirstName}, {nameof(CreatorMiddleName)} = {CreatorMiddleName}, {nameof(UpdaterLastName)} = {UpdaterLastName}, {nameof(UpdaterFirstName)} = {UpdaterFirstName}, {nameof(UpdaterMiddleName)} = {UpdaterMiddleName}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(TemporaryTypeId)} = {TemporaryTypeId}, {nameof(SearchContent)} = {SearchContent}, {nameof(NoteLinks)} = {(NoteLinks != null ? $"{{ {string.Join(", ", NoteLinks)} }}" : "")} }}";

        public static Note3 FromJObject(JObject jObject) => jObject != null ? new Note3
        {
            Id = (Guid?)jObject.SelectToken("id"),
            TypeId = (Guid?)jObject.SelectToken("typeId"),
            Type = (string)jObject.SelectToken("type"),
            Domain = (string)jObject.SelectToken("domain"),
            Title = (string)jObject.SelectToken("title"),
            Content2 = (string)jObject.SelectToken("content"),
            Status = (string)jObject.SelectToken("status"),
            CreatorLastName = (string)jObject.SelectToken("creator.lastName"),
            CreatorFirstName = (string)jObject.SelectToken("creator.firstName"),
            CreatorMiddleName = (string)jObject.SelectToken("creator.middleName"),
            UpdaterLastName = (string)jObject.SelectToken("updater.lastName"),
            UpdaterFirstName = (string)jObject.SelectToken("updater.firstName"),
            UpdaterMiddleName = (string)jObject.SelectToken("updater.middleName"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            NoteLinks = jObject.SelectToken("links")?.Where(jt => jt.HasValues).Select(jt => NoteLink.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("typeId", TypeId),
            new JProperty("type", Type),
            new JProperty("domain", Domain),
            new JProperty("title", Title),
            new JProperty("content", Content2),
            new JProperty("status", Status),
            new JProperty("creator", new JObject(
                new JProperty("lastName", CreatorLastName),
                new JProperty("firstName", CreatorFirstName),
                new JProperty("middleName", CreatorMiddleName))),
            new JProperty("updater", new JObject(
                new JProperty("lastName", UpdaterLastName),
                new JProperty("firstName", UpdaterFirstName),
                new JProperty("middleName", UpdaterMiddleName))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("links", NoteLinks?.Select(nl => nl.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
