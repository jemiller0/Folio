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
    // uc.auth_attempts -> diku_mod_login.auth_attempts
    // AuthAttempt2 -> AuthAttempt
    [DisplayColumn(nameof(Id)), DisplayName("Auth Attempts"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("auth_attempts", Schema = "uc")]
    public partial class AuthAttempt2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.AuthAttempt.json")))
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
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3), JsonProperty("userId")]
        public virtual Guid? UserId { get; set; }

        [Column("last_attempt"), Display(Name = "Last Attempt", Order = 4), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("lastAttempt")]
        public virtual DateTime? LastAttempt { get; set; }

        [Column("attempt_count"), Display(Name = "Attempt Count", Order = 5), JsonProperty("attemptCount")]
        public virtual int? AttemptCount { get; set; }

        [Column("content"), CustomValidation(typeof(AuthAttempt), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 6), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(LastAttempt)} = {LastAttempt}, {nameof(AttemptCount)} = {AttemptCount}, {nameof(Content)} = {Content} }}";

        public static AuthAttempt2 FromJObject(JObject jObject) => jObject != null ? new AuthAttempt2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            UserId = (Guid?)jObject.SelectToken("userId"),
            LastAttempt = (DateTime?)jObject.SelectToken("lastAttempt"),
            AttemptCount = (int?)jObject.SelectToken("attemptCount"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("userId", UserId),
            new JProperty("lastAttempt", LastAttempt?.ToLocalTime()),
            new JProperty("attemptCount", AttemptCount)).RemoveNullAndEmptyProperties();
    }
}
