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
    // uc.ledger_rollover_progresses -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover_progress
    // LedgerRolloverProgress2 -> LedgerRolloverProgress
    [DisplayColumn(nameof(Id)), DisplayName("Ledger Rollover Progresses"), JsonConverter(typeof(JsonPathJsonConverter<LedgerRolloverProgress2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("ledger_rollover_progresses", Schema = "uc")]
    public partial class LedgerRolloverProgress2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.LedgerRolloverProgress.json")))
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

        [Column("overall_rollover_status"), Display(Name = "Overall Rollover Status", Order = 4), JsonProperty("overallRolloverStatus"), RegularExpression(@"^(Not Started|In Progress|Error|Success)$"), StringLength(1024)]
        public virtual string OverallRolloverStatus { get; set; }

        [Column("budgets_closing_rollover_status"), Display(Name = "Budgets Closing Rollover Status", Order = 5), JsonProperty("budgetsClosingRolloverStatus"), RegularExpression(@"^(Not Started|In Progress|Error|Success)$"), StringLength(1024)]
        public virtual string BudgetsClosingRolloverStatus { get; set; }

        [Column("financial_rollover_status"), Display(Name = "Financial Rollover Status", Order = 6), JsonProperty("financialRolloverStatus"), RegularExpression(@"^(Not Started|In Progress|Error|Success)$"), StringLength(1024)]
        public virtual string FinancialRolloverStatus { get; set; }

        [Column("orders_rollover_status"), Display(Name = "Orders Rollover Status", Order = 7), JsonProperty("ordersRolloverStatus"), RegularExpression(@"^(Not Started|In Progress|Error|Success)$"), StringLength(1024)]
        public virtual string OrdersRolloverStatus { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 9), InverseProperty("LedgerRolloverProgress2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 10), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 13), InverseProperty("LedgerRolloverProgress2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 14), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(LedgerRolloverProgress), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 16), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(LedgerRolloverId)} = {LedgerRolloverId}, {nameof(OverallRolloverStatus)} = {OverallRolloverStatus}, {nameof(BudgetsClosingRolloverStatus)} = {BudgetsClosingRolloverStatus}, {nameof(FinancialRolloverStatus)} = {FinancialRolloverStatus}, {nameof(OrdersRolloverStatus)} = {OrdersRolloverStatus}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static LedgerRolloverProgress2 FromJObject(JObject jObject) => jObject != null ? new LedgerRolloverProgress2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            LedgerRolloverId = (Guid?)jObject.SelectToken("ledgerRolloverId"),
            OverallRolloverStatus = (string)jObject.SelectToken("overallRolloverStatus"),
            BudgetsClosingRolloverStatus = (string)jObject.SelectToken("budgetsClosingRolloverStatus"),
            FinancialRolloverStatus = (string)jObject.SelectToken("financialRolloverStatus"),
            OrdersRolloverStatus = (string)jObject.SelectToken("ordersRolloverStatus"),
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
            new JProperty("ledgerRolloverId", LedgerRolloverId),
            new JProperty("overallRolloverStatus", OverallRolloverStatus),
            new JProperty("budgetsClosingRolloverStatus", BudgetsClosingRolloverStatus),
            new JProperty("financialRolloverStatus", FinancialRolloverStatus),
            new JProperty("ordersRolloverStatus", OrdersRolloverStatus),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
