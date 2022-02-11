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
    // uc.groups -> uchicago_mod_users.groups
    // Group2 -> Group
    [CustomValidation(typeof(Group2), nameof(ValidateGroup2)), DisplayColumn(nameof(Name)), DisplayName("Groups"), JsonConverter(typeof(JsonPathJsonConverter<Group2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("groups", Schema = "uc")]
    public partial class Group2
    {
        public static ValidationResult ValidateGroup2(Group2 group2, ValidationContext context)
        {
            var fsc = (FolioServiceContext)context.ObjectInstance;
            if (group2.Name != null && fsc.AnyGroup2s($"id <> \"{group2.Id}\" and group == \"{group2.Name}\"")) return new ValidationResult("Name already exists");
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Group.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("group"), Display(Order = 2), JsonProperty("group"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("desc"), Display(Order = 3), JsonProperty("desc"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("expiration_offset_in_days"), Display(Name = "Expiration Offset In Days", Order = 4), JsonProperty("expirationOffsetInDays")]
        public virtual int? ExpirationOffsetInDays { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 5), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 6), InverseProperty("Group2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 7), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 10), InverseProperty("Group2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 11), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Group), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 13), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Block Limits", Order = 14)]
        public virtual ICollection<BlockLimit2> BlockLimit2s { get; set; }

        [Display(Name = "Loans", Order = 15)]
        public virtual ICollection<Loan2> Loan2s { get; set; }

        [Display(Name = "Users", Order = 16)]
        public virtual ICollection<User2> User2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Description)} = {Description}, {nameof(ExpirationOffsetInDays)} = {ExpirationOffsetInDays}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static Group2 FromJObject(JObject jObject) => jObject != null ? new Group2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("group"),
            Description = (string)jObject.SelectToken("desc"),
            ExpirationOffsetInDays = (int?)jObject.SelectToken("expirationOffsetInDays"),
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
            new JProperty("group", Name),
            new JProperty("desc", Description),
            new JProperty("expirationOffsetInDays", ExpirationOffsetInDays),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
