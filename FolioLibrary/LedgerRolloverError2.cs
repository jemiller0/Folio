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
    // uc.ledger_rollover_errors -> diku_mod_finance_storage.ledger_fiscal_year_rollover_error
    // LedgerRolloverError2 -> LedgerRolloverError
    [DisplayColumn(nameof(Id)), DisplayName("Ledger Rollover Errors"), JsonConverter(typeof(JsonPathJsonConverter<LedgerRolloverError2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("ledger_rollover_errors", Schema = "uc")]
    public partial class LedgerRolloverError2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.LedgerRolloverError.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Ledger Rollover", Order = 2)]
        public virtual LedgerRollover2 LedgerRollover { get; set; }

        [Column("ledger_rollover_id"), Display(Name = "Ledger Rollover", Order = 3), JsonProperty("ledgerRolloverId")]
        public virtual Guid? LedgerRolloverId { get; set; }

        [Column("error_type"), Display(Name = "Error Type", Order = 4), JsonProperty("errorType"), RegularExpression(@"^(Fund|Order)$"), StringLength(1024)]
        public virtual string ErrorType { get; set; }

        [Column("failed_action"), Display(Name = "Failed Action", Order = 5), JsonProperty("failedAction"), StringLength(1024)]
        public virtual string FailedAction { get; set; }

        [Column("error_message"), Display(Name = "Error Message", Order = 6), JsonProperty("errorMessage"), StringLength(1024)]
        public virtual string ErrorMessage { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 7), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 8), InverseProperty("LedgerRolloverError2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 9), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 12), InverseProperty("LedgerRolloverError2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 13), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(LedgerRolloverError), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 15), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(LedgerRolloverId)} = {LedgerRolloverId}, {nameof(ErrorType)} = {ErrorType}, {nameof(FailedAction)} = {FailedAction}, {nameof(ErrorMessage)} = {ErrorMessage}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static LedgerRolloverError2 FromJObject(JObject jObject) => jObject != null ? new LedgerRolloverError2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            LedgerRolloverId = (Guid?)jObject.SelectToken("ledgerRolloverId"),
            ErrorType = (string)jObject.SelectToken("errorType"),
            FailedAction = (string)jObject.SelectToken("failedAction"),
            ErrorMessage = (string)jObject.SelectToken("errorMessage"),
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
            new JProperty("ledgerRolloverId", LedgerRolloverId),
            new JProperty("errorType", ErrorType),
            new JProperty("failedAction", FailedAction),
            new JProperty("errorMessage", ErrorMessage),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
