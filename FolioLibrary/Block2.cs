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
    // uc.blocks -> diku_mod_feesfines.manualblocks
    // Block2 -> Block
    [DisplayColumn(nameof(Id)), DisplayName("Blocks"), JsonConverter(typeof(JsonPathJsonConverter<Block2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("blocks", Schema = "uc")]
    public partial class Block2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Block.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("type"), Display(Order = 2), JsonProperty("type"), StringLength(1024)]
        public virtual string Type { get; set; }

        [Column("desc"), Display(Order = 3), JsonProperty("desc"), Required, StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("code"), Display(Order = 4), JsonProperty("code"), StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("staff_information"), Display(Name = "Staff Information", Order = 5), JsonProperty("staffInformation"), StringLength(1024)]
        public virtual string StaffInformation { get; set; }

        [Column("patron_message"), Display(Name = "Patron Message", Order = 6), JsonProperty("patronMessage"), StringLength(1024)]
        public virtual string PatronMessage { get; set; }

        [Column("expiration_date"), DataType(DataType.Date), Display(Name = "Expiration Date", Order = 7), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("expirationDate")]
        public virtual DateTime? ExpirationDate { get; set; }

        [Column("borrowing"), Display(Order = 8), JsonProperty("borrowing")]
        public virtual bool? Borrowing { get; set; }

        [Column("renewals"), Display(Order = 9), JsonProperty("renewals")]
        public virtual bool? Renewals { get; set; }

        [Column("requests"), Display(Order = 10), JsonProperty("requests")]
        public virtual bool? Requests { get; set; }

        [Display(Order = 11), InverseProperty("Block2s2")]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 12), JsonProperty("userId"), Required]
        public virtual Guid? UserId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 14), InverseProperty("Block2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 15), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 17), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 18), InverseProperty("Block2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 19), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Block), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 21), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Type)} = {Type}, {nameof(Description)} = {Description}, {nameof(Code)} = {Code}, {nameof(StaffInformation)} = {StaffInformation}, {nameof(PatronMessage)} = {PatronMessage}, {nameof(ExpirationDate)} = {ExpirationDate}, {nameof(Borrowing)} = {Borrowing}, {nameof(Renewals)} = {Renewals}, {nameof(Requests)} = {Requests}, {nameof(UserId)} = {UserId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static Block2 FromJObject(JObject jObject) => jObject != null ? new Block2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Type = (string)jObject.SelectToken("type"),
            Description = (string)jObject.SelectToken("desc"),
            Code = (string)jObject.SelectToken("code"),
            StaffInformation = (string)jObject.SelectToken("staffInformation"),
            PatronMessage = (string)jObject.SelectToken("patronMessage"),
            ExpirationDate = (DateTime?)jObject.SelectToken("expirationDate"),
            Borrowing = (bool?)jObject.SelectToken("borrowing"),
            Renewals = (bool?)jObject.SelectToken("renewals"),
            Requests = (bool?)jObject.SelectToken("requests"),
            UserId = (Guid?)jObject.SelectToken("userId"),
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
            new JProperty("type", Type),
            new JProperty("desc", Description),
            new JProperty("code", Code),
            new JProperty("staffInformation", StaffInformation),
            new JProperty("patronMessage", PatronMessage),
            new JProperty("expirationDate", ExpirationDate?.ToLocalTime()),
            new JProperty("borrowing", Borrowing),
            new JProperty("renewals", Renewals),
            new JProperty("requests", Requests),
            new JProperty("userId", UserId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
